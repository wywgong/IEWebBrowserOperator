using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IEWebBrowserOperator;

namespace GDAccountAutoGenerate
{
    public class GdAccountAutoGenerate
    {
        private readonly WebBrowserUtility _webBrowserUtility;

        public GdAccountAutoGenerate(WebBrowser webBrowser)
        {
            _webBrowserUtility = new WebBrowserUtility(webBrowser);
        }

        public void ActivateCard(GdCard card)
        {
            throw new NotImplementedException();
        }

        public void GetCardPin(GdEnvironment environment, string productCode, int pinValue)
        {
            throw new NotImplementedException();
        }

        public void RegisterCard(GdCard card)
        {
            //Step 1: Navigate to register card page
            _webBrowserUtility
                .NavigateAsync("http://www.greendot.com/greendot/activation/online-activation-init")
                .ContinueWith(
                              task =>
                              {
                                  //Step 2:Fill the Card info
                                  var scriptTxt = File.ReadAllText("Script/Register/step1FillCardInfo.js");
                                  _webBrowserUtility.ExecuteJavascript(scriptTxt);
                              })
                .ContinueWith(
                              task =>
                              {

                              })
                .ContinueWith(
                              task =>
                              {

                              })
                ;
        }
    }

    public class GdCard
    {
        public string AccountNumber { get; private set; }
        public string CVV { get; private set; }

        public GdCard(string accountNumber, string cvv)
        {
            AccountNumber = accountNumber;
            CVV = cvv;
        }
    }

    public enum GdEnvironment
    {
        DevInt = 4,
        Di2 = 9,
        Qa2 = 2,
        Qa3 = 1,
        Qa4 = 6,
        Qa5 = 10,
        Staging = 5
    }
}