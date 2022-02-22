using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace TCGEpicor
{
    public partial class SolidWorksViewer : Form
    {
        public List<string> filesToPrint { get; set; }
        public string printerName { get; set; }

        public SolidWorksViewer()
        {
            InitializeComponent();
            axEModelViewControl1.OnFinishedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEventHandler(axEModelViewControl1_OnFinishedLoadingDocument);
            axEModelViewControl1.OnFinishedPrintingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedPrintingDocumentEventHandler(axEModelViewControl1_OnFinishedPrintingDocument);
            axEModelViewControl1.OnFailedPrintingDocument += new AxEModelView._IEModelViewControlEvents_OnFailedPrintingDocumentEventHandler(axEModelViewControl1_OnFailedPrintingDocument);
            axEModelViewControl1.OnFailedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFailedLoadingDocumentEventHandler(axEModelViewControl1_OnFailedLoadingDocument);
        }

        void axEModelViewControl1_OnFailedLoadingDocument(object sender, AxEModelView._IEModelViewControlEvents_OnFailedLoadingDocumentEvent e)
        {
            MessageBox.Show("Error loading file: " + e.fileName, "Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            PrintNextDoc();
        }

        void axEModelViewControl1_OnFailedPrintingDocument(object sender, AxEModelView._IEModelViewControlEvents_OnFailedPrintingDocumentEvent e)
        {
            MessageBox.Show("Error printing file: " + e.printJobName, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            PrintNextDoc();
        }

        void axEModelViewControl1_OnFinishedPrintingDocument(object sender, AxEModelView._IEModelViewControlEvents_OnFinishedPrintingDocumentEvent e)
        {
            axEModelViewControl1.CloseActiveDoc("");
            PrintNextDoc();
        }

        void axEModelViewControl1_OnFinishedLoadingDocument(object sender, AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEvent e)
        {
            string ext = e.fileName.Substring(e.fileName.LastIndexOf('.') + 1);
            axEModelViewControl1.SetPageSetupOptions(EModelView.EMVPrintOrientation.eLandscape, 1, 0, 0, 1, 7, printerName, 0, 0, 0, 0);
            EModelView.EMVPrintType printtype;
            if (ext.ToLower() == "sldprt")
                printtype = EModelView.EMVPrintType.eWYSIWYG;
            else
                printtype = EModelView.EMVPrintType.eScaleToFit;
//            axEModelViewControl1.Print5(false, e.fileName, false, true, false, EModelView.EMVPrintType.eWYSIWYG, 0, 0, 0, true, 0, 0, "");
            axEModelViewControl1.Print5(false, e.fileName, true, false, true, printtype, 0, 0, 0, true, 0, 0, "");
        }

        private void SolidWorksViewer_Shown(object sender, EventArgs e)
        {
            PrintNextDoc();
        }

        private void PrintNextDoc()
        {
            if (filesToPrint.Count > 0)
            {
                    axEModelViewControl1.OpenDoc(filesToPrint[0], false, false, true, "");
                    filesToPrint.RemoveAt(0);
            }
            else
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

    }
}
