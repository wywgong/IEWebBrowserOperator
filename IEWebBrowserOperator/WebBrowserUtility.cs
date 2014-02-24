using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;

namespace IEWebBrowserOperator
{
    public class WebBrowserUtility
    {
        private readonly WebBrowser _browser;
        private readonly TaskScheduler _currentScheduler;
        private readonly TaskFactory _currentTaskFactory;
        private readonly TaskFactory _defaultTaskFactory;
        private readonly TaskScheduler _defaultScheduler;
        private readonly AutoResetEvent _syncEvent = new AutoResetEvent(false);
        private string _documentHtml = "";
        private string _documentTitle = "";

        public void ExecuteJavascript(string script)
        {
            _currentTaskFactory
                .StartNew(
                          () =>
                          {
                              HtmlElement head = _browser.Document.GetElementsByTagName("head")[0];
                              HtmlElement scriptElement = _browser.Document.CreateElement("script");
                              IHTMLScriptElement element = (IHTMLScriptElement) scriptElement.DomElement;
                              element.text = string.Format("function injectScript(){{{0}}}", script);
                              head.AppendChild(scriptElement);
                              _browser.Document.InvokeScript("injectScript");
                          });
            

        }
        public string DocumentHtml
        {
            get
            {
                return _documentHtml;
            }
        }

        public string DocumentTitle
        {
            get
            {
                return _documentTitle;
            }
        }

        public WebBrowserUtility()
        {
            _currentScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            _currentTaskFactory = new TaskFactory(_currentScheduler);
            _defaultScheduler = TaskScheduler.Default;
            _defaultTaskFactory = new TaskFactory(_defaultScheduler);
            _browser = new WebBrowser();
            _browser.DocumentCompleted += BrowserOnDocumentCompleted;
        }

        public WebBrowserUtility(WebBrowser webBrowser)
        {
            _currentScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            _currentTaskFactory = new TaskFactory(_currentScheduler);
            _defaultScheduler = TaskScheduler.Default;
            _defaultTaskFactory = new TaskFactory(_defaultScheduler);
            _browser = webBrowser;
            _browser.DocumentCompleted += BrowserOnDocumentCompleted;
        }

        private void Navigate(string url)
        {
            _currentTaskFactory.StartNew(() => _browser.Navigate(url));
        }


        public Task<string> NavigateAsync(string url)
        {
            return _defaultTaskFactory.StartNew(
                                                () =>
                                                {
                                                    Navigate(url);
                                                    _syncEvent.WaitOne();
                                                    _syncEvent.Reset();
                                                    return _documentHtml;
                                                });
        }

        private void RefreshDocumentContent()
        {
            _currentTaskFactory.StartNew(
                                         () =>
                                         {
                                             _documentHtml = _browser.DocumentText;
                                             _documentTitle = _browser.DocumentTitle;
                                         });
        }
        public Task<string> WaitUntil(string regexPattern)
        {
            return _defaultTaskFactory.StartNew(
                                                () =>
                                                {
                                                    RefreshDocumentContent();
                                                    if (!Regex.IsMatch(_documentHtml, regexPattern))
                                                    {
                                                        Thread.Sleep(1000);
                                                    }
                                                    return _documentHtml;
                                                });

        }
        
        private void ClickSpecifyElement(string id)
        {
            _currentTaskFactory
                .StartNew(
                          () =>
                          {
                              var element = _browser.Document.GetElementById(id);
                              element.InvokeMember("click");
                          });
        }

        public Task<string> ClickSpecifyElementAsync(string id)
        {
            return _defaultTaskFactory.StartNew(
                                                () =>
                                                {
                                                    ClickSpecifyElement(id);
                                                    _syncEvent.WaitOne();
                                                    _syncEvent.Reset();
                                                    return _documentHtml;
                                                });
        }
        public void SetFieldValue(string id, string value)
        {
            //var allElements = _browser.Document.All.Cast<HtmlElement>();
            var element = _browser.Document.GetElementById(id);
            element.SetAttribute("value", value);
        }

        private void BrowserOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs eventArgs)
        {
            _documentHtml = _browser.DocumentText;
            _documentTitle = _browser.DocumentTitle;
            _syncEvent.Set();
        }
    }
}