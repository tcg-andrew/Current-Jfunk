#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using Ice.Core;
using Ice.Lib.Framework;
using Erp.Proxy.BO;
using Erp.BO;
using Erp.Contracts;
using Microsoft.Win32;
using System.ComponentModel;
using System.Drawing;
using System.IO.IsolatedStorage;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Diagnostics;
using PdfSharp.Pdf.Printing;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Text;
using System.Security.Principal;
using Microsoft.Reporting.WinForms;
using OfficeOpenXml;
using CrystalDecisions.CrystalReports.Engine;
using System.Net;
using OfficeOpenXml.Style;

#endregion

namespace TCGEpicor
{
    public partial class Form1 : Form
    {
        #region Values

        Dictionary<string, Module> Modules;
        delegate void controlMethod();
        Session session;
        string VantageServer;
        string Server;
        string Database;

        string LegacyServer;
        string LegacyDatabase;

        DataCapture dc;
        SolidWorksViewer swView;
        WindowsPrincipal wp;

        #endregion

        /* Add module setup to CreateModules */
        #region Constructors

        public Form1()
        {
            /*            SolidWorksViewer = new AxEModelView.AxEModelViewControl();
                        SolidWorksViewer.OnFinishedLoadingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedLoadingDocumentEventHandler(Form1_OnFinishedLoadingDocument);
                        SolidWorksViewer.OnFinishedPrintingDocument += new AxEModelView._IEModelViewControlEvents_OnFinishedPrintingDocumentEventHandler(Form1_OnFinishedPrintingDocument);
                        this.Controls.Add(SolidWorksViewer);
                        SolidWorksViewer.Show();
            */
            InitializeComponent();
            wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());

            CreateModules();
            DrawDashboard();
        }

        public Form1(string vs, string s, string d)
        {
            //            SolidWorksViewer = new AxEModelView.AxEModelViewControl();
            InitializeComponent();
            wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            VantageServer = vs;
            Server = s; ;
            Database = d;
            CreateModules();
            DrawDashboard();
        }

        public Form1(string vs, string s, string d, string ls, string ld)
        {
            InitializeComponent();
            wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            VantageServer = vs;
            Server = s;
            Database = d;
            LegacyServer = ls;
            LegacyDatabase = ld;
            CreateModules();
            DrawDashboard();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (session != null)
                session.Dispose();

            base.OnClosing(e);
        }

        private void CreateModules()
        {
            Modules = new Dictionary<string, Module>();

            #region Completed Labor Operations

            Modules.Add("Completed Labor Operations", new Module(
                new List<Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Refresh,
                    TCGEpicor.Module.ModuleAction.Favorite
                },
                new List<GridColumn>()
                {
                    new GridColumn("Job #"),
                    new GridColumn("Asm #"),
                    new GridColumn("Opr"),
                    new GridColumn("Part #"),
                    new GridColumn("Qty"),
                    new GridColumn("Emp #"),
                    new GridColumn("Group")

                }));

            Modules["Completed Labor Operations"].PreLoadDataCaptures.Add("Company", new Dictionary<string, string>() { { "CRD", "CRD" }, { "CIG", "CIG" } });
            Modules["Completed Labor Operations"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>() { { "FL", "SFL" }, { "TN", "CTN" } });

            Modules["Completed Labor Operations"].PreLoadDataCaptures.Add("From (MM/DD/YYYY) *blank = yesterday", new Dictionary<string, string>());
            Modules["Completed Labor Operations"].PreLoadDataCaptures.Add("To   (MM/DD/YYYY) *blank = yesterday", new Dictionary<string, string>());

            #endregion

            #region Part Usage

            Modules.Add("Part Usage", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Refresh,
                    TCGEpicor.Module.ModuleAction.Favorite
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #", true, true),
                    new GridColumn("Description"),
                    new GridColumn("Class"),
                    new GridColumn("Last Usage"),
                    new GridColumn("Last Purchase"),
                    new GridColumn("1mo Usage"),
                    new GridColumn("3mo Usage"),
                    new GridColumn("6mo Usage"),
                    new GridColumn("12mo Usage"),
                    new GridColumn("12mo Usage Cost"),
                    new GridColumn("12mo Job Mtl Cost"),
                    new GridColumn("24mo Usage"),
                    new GridColumn("24mo Usage Cost"),
                    new GridColumn("24mo Job Mtl Cost"),
                    new GridColumn("36mo Usage"),
                    new GridColumn("36mo Usage Cost"),
                    new GridColumn("36mo Job Mtl Cost"),
                    new GridColumn("On Hand"),
                    new GridColumn("Avg Cost"),
                    new GridColumn("Pur Last 12mo"),
                    new GridColumn("Adj Last 12mo")
                }));

            Modules["Part Usage"].PreLoadDataCaptures.Add("Company", new Dictionary<string, string>() { { "CRD", "CRD" }, { "CIG", "CIG" } });
            Modules["Part Usage"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>() { { "FL", "SFL" }, { "TN", "CTN" } });
            Modules["Part Usage"].PreLoadDataCaptures.Add("As Of (MM/DD/YYYY)", new Dictionary<string, string>());

            #endregion

            #region Customer Summary

            Modules.Add("Customer Summary", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Refresh,
                    TCGEpicor.Module.ModuleAction.Favorite
                },
                new List<GridColumn>() { }));

            #endregion

            #region Method Tracker

            Modules.Add("Method Tracker", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Refresh,
                    TCGEpicor.Module.ModuleAction.Favorite
                },
                new List<GridColumn>()
                {
                }
                ));

            Modules["Method Tracker"].PreLoadDataCaptures.Add("Invoice #s (comma seperated)", new Dictionary<string, string>());
            Modules["Method Tracker"].PreLoadDataCaptures.Add("Burden Factor", new Dictionary<string, string>());
            Modules["Method Tracker"].PreLoadDataCaptures.Add("Matl Burden Factor", new Dictionary<string, string>());

            #endregion

            #region Spare Parts Report

            {
                Modules.Add("Spare Parts Report", new Module(
                    new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                    new List<GridColumn>()
                {
                    new GridColumn("Part #"),
                    new GridColumn("Description"),
                    new GridColumn("$ Sold"),
                    new GridColumn("Qty Sold"),
                    new GridColumn("Latest Part Cost"),
                    new GridColumn("Last Purchase/Fabricate Date"),
                    new GridColumn("Extended Cost"),
                    new GridColumn("Net Margin $")
                }
                    ));
                GridColumn gc = new GridColumn("Net Margin %");
                gc.DataType = typeof(Decimal);
                gc.CellFormat = "0%";
                Modules["Spare Parts Report"].Columns.Add(gc);

                Modules["Spare Parts Report"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
                Modules["Spare Parts Report"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());
                Modules["Spare Parts Report"].PreLoadDataCaptures.Add("Part List (comma seperated)", new Dictionary<string, string>());
            }
            #endregion

            #region Missing Part Checklist

            Modules.Add("Missing Part Checklist", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh,
                    TCGEpicor.Module.ModuleAction.Print
                },
                new List<GridColumn>()
                {
                    new GridColumn("Job #"), 
                    new GridColumn("Mtl Part #"), 
                    new GridColumn("Req Qty"), 
                    new GridColumn("Issued Qty"),
                    new GridColumn("Assembly"),
                    new GridColumn("Operation")
                }
                ));
            Modules["Missing Part Checklist"].Columns[4].Visible = false;
            Modules["Missing Part Checklist"].Columns[5].Visible = false;
            Modules["Missing Part Checklist"].PreLoadDataCaptures.Add("Job #", new Dictionary<string, string>());
            Modules["Missing Part Checklist"].PreLoadDataCaptures.Add("Missing Only", new Dictionary<string, string>() { { "Yes", "Yes" }, { "No", "No" } });

            #endregion

            #region SO Backlock

            Modules.Add("SO Backlog", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Order Date"), 
                    new GridColumn("Order #"), 
                    new GridColumn("Order Line #"), 
                    new GridColumn("Order Rel #"), 
                    new GridColumn("Part #"), 
                    new GridColumn("Quantity"), 
                    new GridColumn("Unit Price"), 
                    new GridColumn("Ext Price"), 
                    new GridColumn("Ship Date")
                }
                ));

            Modules["SO Backlog"].PreLoadDataCaptures.Add("As Of Date (mm/dd/yyyy)", new Dictionary<string, string>());

            #endregion

            #region Part Bins Not Primary

            Modules.Add("Part Bins Not Primary", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #"),
                    new GridColumn("Plant"),
                    new GridColumn("Warehouse"),
                    new GridColumn("Primary Bin"),
                    new GridColumn("Secondary Bin")
                }
                ));

            #endregion

            #region Contact Email List

            Modules.Add("Contact Email List", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite
                },
                new List<GridColumn>()
                {
                    new GridColumn("Contact Name"),
                    new GridColumn("Email Address")
                }
                ));

            #endregion

            #region Customer Info

            Modules.Add("Customer Info", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Print,
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Cust ID"),
                    new GridColumn("Name"),
                    new GridColumn("City"),
                    new GridColumn("State"),
                    new GridColumn("Zip"),
                    new GridColumn("Country")
                }
                ));
            Modules["Customer Info"].Filters.Add("City", new Dictionary<string, string>() { { "All", "%" } });
            Modules["Customer Info"].SelectedFilter["City"] = "All";
            Modules["Customer Info"].Filters.Add("State", new Dictionary<string, string>() { { "All", "%" } });
            Modules["Customer Info"].SelectedFilter["State"] = "All";
            Modules["Customer Info"].Filters.Add("Country", new Dictionary<string, string>() { { "All", "%" } });
            Modules["Customer Info"].SelectedFilter["Country"] = "All";

            #endregion

            #region Customers with no SO in time frame

            Modules.Add("Customers w/ No SO", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Cust ID"),
                    new GridColumn("Name")
                }
                ));
            Modules["Customers w/ No SO"].PreLoadDataCaptures.Add("Months Period", new Dictionary<string, string>());
            Modules["Customers w/ No SO"].PreLoadDataCaptures.Add("Customer Type", new Dictionary<string, string>());

            #endregion

            #region Edit Job/Order Dates Module - Ported

            Modules.Add("Edit Job/Order Dates", new Module(
                new List<TCGEpicor.Module.ModuleAction>() 
                {   
                    TCGEpicor.Module.ModuleAction.Save, 
                    TCGEpicor.Module.ModuleAction.Favorite, 
                    TCGEpicor.Module.ModuleAction.Refresh 
                },
                new List<GridColumn>() 
                {   
                    new GridColumn("Order Shop Cap", false), 
                    new GridColumn("Job Req Due Date", false), 
                    new GridColumn("Order #"), 
                    new GridColumn("Order Line #"), 
                    new GridColumn("Order Release #"), 
                    new GridColumn("Job #") 
                }
            ));

            #endregion

            #region Sales Rep Mgmt

            Modules.Add("Sales Rep Mgmt", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Save,
                    TCGEpicor.Module.ModuleAction.Favorite, 
                    TCGEpicor.Module.ModuleAction.Refresh 
                },
                new List<GridColumn>()
                {
                    new GridColumn("Cust ID"),
                    new GridColumn("Customer"),
                    new GridColumn("Sales Rep Code", false),
                    new GridColumn("Message")
                }
            ));

            Modules["Sales Rep Mgmt"].PreLoadDataCaptures.Add("Cust IDs (comma seperated)", new Dictionary<string, string>());

            #endregion

            #region Print Part Attach Files Module

            Modules.Add("Print Part Attach Files For Jobs", new Module(
                new List<TCGEpicor.Module.ModuleAction>() 
                { 
                    TCGEpicor.Module.ModuleAction.Favorite, 
                    TCGEpicor.Module.ModuleAction.Refresh 
                },
                new List<GridColumn>() 
                { 
                    new GridColumn("Job #"), 
                    new GridColumn("Job Part #"), 
                    new GridColumn("Job Status") 
                }
            ));
            Modules["Print Part Attach Files For Jobs"].AllowDrillDown = true;
            Modules["Print Part Attach Files For Jobs"].PreLoadDataCaptures.Add("Search Depth", new Dictionary<string, string>());

            Modules.Add("Print Part Attach Files For MOMs", new Module(
                new List<TCGEpicor.Module.ModuleAction>() 
                { 
                    TCGEpicor.Module.ModuleAction.Favorite, 
                    TCGEpicor.Module.ModuleAction.Refresh 
                },
                new List<GridColumn>() 
                { 
                    new GridColumn("Part #")
                }
            ));
            Modules["Print Part Attach Files For MOMs"].AllowDrillDown = true;

            Modules.Add("Part Attach File List For Jobs", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Print
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #"),
                    new GridColumn("Assembly Seq"),
                    new GridColumn("Operation"),
                    new GridColumn("Mtl Seq"),
                    new GridColumn("Filename")
                }
            ));

            Modules.Add("Part Attach File List For MOMs", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Print
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #"),
                    new GridColumn("Operation"),
                    new GridColumn("Filename")
                }
            ));

            #endregion

            #region Missing Parts List Module

            #region Jobs

            {
                GridColumn gc = new GridColumn("Change Visibility");
                gc.ActionColumn = wp.IsInRole("Epicor - Missing Parts Edit");

                GridColumn gc1 = new GridColumn("Backflush On");
                gc1.ActionColumn = wp.IsInRole("Epicor - Missing Parts Edit");

                GridColumn gc2 = new GridColumn("Backflush Off");
                gc2.ActionColumn = wp.IsInRole("Epicor - Missing Parts Edit");

                Modules.Add("Get Jobs Missing Parts", new Module(
                    new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                }, new List<GridColumn>()
                {
                    gc,
                    gc1,
                    gc2,
                    new GridColumn("Job #"),
                    new GridColumn("Job Part #"),
                    new GridColumn("Job Part Description"),
                    new GridColumn("Visibility")
                }));
                Modules["Get Jobs Missing Parts"].AllowDrillDown = true;
                Modules["Get Jobs Missing Parts"].Filters.Add("Visibility", new Dictionary<string, string>() { { "All", "%" }, { "Visible", "VISIBLE" }, { "Hidden", "HIDDEN" } });
                Modules["Get Jobs Missing Parts"].SelectedFilter["Visibility"] = "Visible";
            }

            {
                List<GridColumn> gcs = new List<GridColumn>();
                GridColumn gc = new GridColumn("Change Visibility");
                gc.ActionColumn = wp.IsInRole("Epicor - Missing Parts Edit");
                gcs.Add(gc);
                gcs.AddRange(new List<GridColumn>()
                                    { 
                        new GridColumn("Job #"), 
                        new GridColumn("Assembly Seq"),
                        new GridColumn("Operation"),
                        new GridColumn("Mtl Seq"),
                        new GridColumn("Mtl Bin"),
                        new GridColumn("Mtl Partnum"),
                        new GridColumn("Mtl Pull Qty"),
                        new GridColumn("Mtl Part Type"),
                        new GridColumn("Mtl Normally Purchased")
                                    });

                gc = new GridColumn("Mtl Comment");
                gc.EditTextAction = true;
                gc.ReadOnly = false;
                gcs.Add(gc);
                gcs.AddRange(new List<GridColumn>()
                {
                        new GridColumn("Job Partnum"), 
                        new GridColumn("Job Part Description"), 
                        new GridColumn("Mtl Revision"),
                        new GridColumn("Mtl Warehouse Code"),
                        new GridColumn("Mtl Description"),
                        new GridColumn("Mtl On Hand Qty", typeof(Decimal)), 
                        new GridColumn("Mtl Non Inventory Qty", typeof(Decimal)), 
                        new GridColumn("Mtl Total SO Demand", typeof(Decimal)), 
                        new GridColumn("Mtl Total Job Demand", typeof(Decimal)), 
                        new GridColumn("Mtl Incoming PO", typeof(Int32)), 
                        new GridColumn("Mtl Incoming Job", typeof(Int32)),
                        new GridColumn("Below Req Qty"),
                        new GridColumn("Visibility")
                    }
                    );
                gc = new GridColumn("Backflush");
                gc.Visible = false;
                gcs.Add(gc);
                gc = new GridColumn("Last Backflush");
                gc.Visible = false;
                gcs.Add(gc);
                gc = new GridColumn("Qty Bearing");
                gc.Visible = false;
                gcs.Add(gc);
                gc = new GridColumn("Direct");
                gc.Visible = false;
                gcs.Add(gc);
                gc = new GridColumn("Issued Qty");
                gc.Visible = false;
                gcs.Add(gc);

                Modules.Add("Get Jobs Missing Parts Details", new Module(

                    new List<TCGEpicor.Module.ModuleAction>() 
                    { 
                        TCGEpicor.Module.ModuleAction.Favorite, 
                        TCGEpicor.Module.ModuleAction.Refresh,
                        TCGEpicor.Module.ModuleAction.Print
                    }, gcs));
                Modules["Get Jobs Missing Parts Details"].AllowDrillDown = true;
                Modules["Get Jobs Missing Parts Details"].Filters.Add("Mtl Part Type", new Dictionary<string, string>() { { "All", "%" }, { "Manufactured", "M" }, { "Produced", "P" } });
                Modules["Get Jobs Missing Parts Details"].SelectedFilter["Mtl Part Type"] = "All";
                Modules["Get Jobs Missing Parts Details"].Filters.Add("Visibility", new Dictionary<string, string>() { { "All", "%" }, { "Visible", "VISIBLE" }, { "Hidden", "HIDDEN" } });
                Modules["Get Jobs Missing Parts Details"].SelectedFilter["Visibility"] = "Visible";
                Modules["Get Jobs Missing Parts Details"].PreLoadDataCaptures.Add("Issue/Return?", new Dictionary<string, string>());
            }


            /*            {
                            List<GridColumn> gcs = new List<GridColumn>();
                            GridColumn gc = new GridColumn("Change Visibility");
                            gc.ActionColumn = true;
                            gcs.Add(gc);
                            gcs.AddRange(new List<GridColumn>() 
                            { 
                                new GridColumn("Partnum"), 
                                new GridColumn("Part Type"), 
                                new GridColumn("Normally Purchased"),
                                new GridColumn("Revision"), 
                                new GridColumn("Warehouse Code"), 
                                new GridColumn("Description"), 
                                new GridColumn("On Hand Qty", typeof(Decimal)), 
                                new GridColumn("Non Inventory Qty", typeof(Decimal)), 
                                new GridColumn("Total SO Demand", typeof(Decimal)), 
                                new GridColumn("Total Job Demand", typeof(Decimal)), 
                                new GridColumn("Incoming PO", typeof(Int32)), 
                                new GridColumn("Incoming Job", typeof(Int32)), 
                                new GridColumn("Visibility") 
                            });
                            Modules.Add("Get Parts Below Req Qty", new Module(gcs));
                            Modules["Get Parts Below Req Qty"].AllowDrillDown = true;
                            Modules["Get Parts Below Req Qty"].Filters.Add("Part Type", new Dictionary<string, string>() { { "All", "%" }, { "Manufactured", "M" }, { "Produced", "P" } });
                            Modules["Get Parts Below Req Qty"].Filters.Add("Visibility", new Dictionary<string, string>() { { "All", "%" }, { "Visible", "VISIBLE" }, { "Hidden", "HIDDEN" } });
                        }
            */
            #endregion

            #region Orders

            Modules.Add("Get Sales Orders Missing Parts", new Module(
                new List<TCGEpicor.Module.ModuleAction>() 
                { 
                    TCGEpicor.Module.ModuleAction.Favorite, 
                    TCGEpicor.Module.ModuleAction.Refresh 
                },
                new List<GridColumn>() 
                { 
                    new GridColumn("Order #"), 
                    new GridColumn("Order Line"), 
                    new GridColumn("Order Rel #"), 
                    new GridColumn("Req Date"), 
                    new GridColumn("Partnum"), 
                    new GridColumn("Revision"), 
                    new GridColumn("Part Type"), 
                    new GridColumn("Normally Purchased"),
                    new GridColumn("Description") 
                }
            ));
            Modules["Get Sales Orders Missing Parts"].AllowDrillDown = true;

            {
                List<GridColumn> gcs = new List<GridColumn>();
                GridColumn gc = new GridColumn("Change Visibility");
                gc.ActionColumn = wp.IsInRole("Epicor - Missing Parts Edit");
                gcs.Add(gc);
                gcs.AddRange(new List<GridColumn>() 
                { 
                    new GridColumn("Partnum"), 
                    new GridColumn("Revision"), 
                    new GridColumn("Warehouse Code"), 
                    new GridColumn("Description"), 
                    new GridColumn("On Hand Qty", typeof(Decimal)), 
                    new GridColumn("Non Inventory Qty", typeof(Decimal)), 
                    new GridColumn("Total SO Demand", typeof(Decimal)), 
                    new GridColumn("Total Job Demand", typeof(Decimal)), 
                    new GridColumn("Incoming PO", typeof(Int32)), 
                    new GridColumn("Incoming Job", typeof(Int32)), 
                    new GridColumn("Visibility") 
                });
                Modules.Add("Get Order Parts Below Req Qty", new Module(gcs));
                Modules["Get Order Parts Below Req Qty"].AllowDrillDown = true;
                Modules["Get Order Parts Below Req Qty"].Filters.Add("Visibility", new Dictionary<string, string>() { { "All", "%" }, { "Visible", "VISIBLE" }, { "Hidden", "HIDDEN" } });
            }

            #endregion

            Modules.Add("Get Parts Below Req Qty Details", new Module(
                new List<GridColumn>() 
                { 
                    new GridColumn("Partnum"), 
                    new GridColumn("Revision"), 
                    new GridColumn("Warehouse"), 
                    new GridColumn("PO #"), 
                    new GridColumn("PO Source"), 
                    new GridColumn("PO Qty"), 
                    new GridColumn("PO Due Date"), 
                    new GridColumn("Job #"), 
                    new GridColumn("Job Source"), 
                    new GridColumn("Job Qty"), 
                    new GridColumn("Job Due Date") 
                }
            ));

            #endregion

            #region Job Manuals Module

            Modules.Add("Get Job Manuals", new Module(
                new List<TCGEpicor.Module.ModuleAction>() 
                { 
                    TCGEpicor.Module.ModuleAction.Favorite, 
                    TCGEpicor.Module.ModuleAction.Refresh 
                },
                new List<GridColumn>() 
                { 
                    new GridColumn("Job #"), 
                    new GridColumn("Job Status"), 
                    new GridColumn("Part"), 
                    new GridColumn("Description"), 
                    new GridColumn("Quantity"), 
                    new GridColumn("Customer") 
                }
            ));
            Modules["Get Job Manuals"].AllowDrillDown = true;
            Modules["Get Job Manuals"].Filters.Add("Job Status", new Dictionary<string, string>() { { "All", "%" }, { "Open", "Open" }, { "Closed", "Closed" } });
            Modules["Get Job Manuals"].SelectedFilter["Job Status"] = "All";

            Modules.Add("Job Manual Page 1", new Module(
                new List<GridColumn>() 
                { 
                    new GridColumn("Sub Number"), 
                    new GridColumn("Description"), 
                    new GridColumn("Quantity") 
                }
            ));

            Modules.Add("Job Manual Page 2", new Module(
                new List<GridColumn>() 
                { 
                    new GridColumn("Sub Number"), 
                    new GridColumn("Part Description"), 
                    new GridColumn("Quantity"), 
                    new GridColumn("Part Number") 
                }
            ));

            #endregion

            #region Assemblies w/ No Auto Rec Module - Ported

            Modules.Add("Assemblies w/ No Auto Rec", new Module(
                new List<TCGEpicor.Module.ModuleAction>() 
                { 
                    TCGEpicor.Module.ModuleAction.Favorite, 
                    TCGEpicor.Module.ModuleAction.Refresh 
                },
                new List<GridColumn>() 
                { 
                    new GridColumn("Job #"), 
                    new GridColumn("Assembly Seq"), 
                    new GridColumn("Auto Receive Opr"), 
                    new GridColumn("Final Opr"), 
                    new GridColumn("Highest Opr Seq") 
                }
            ));

            #endregion

            #region Purchase Receipt Summaries Module

            {
                Modules.Add("Purchase Receipt Summaries", new Module(
                    new List<TCGEpicor.Module.ModuleAction>() 
                { 
                    TCGEpicor.Module.ModuleAction.Favorite, 
                    TCGEpicor.Module.ModuleAction.Refresh 
                },
                    new List<GridColumn>() 
                { 
                    new GridColumn("G/L Account"), 
                    new GridColumn("G/L Account Description") 
                }
                ));
                GridColumn gc = new GridColumn("Total Receipt $");
                gc.DataType = typeof(Decimal);
                gc.CellFormat = "c";
                Modules["Purchase Receipt Summaries"].Columns.Add(gc);
                Modules["Purchase Receipt Summaries"].AllowDrillDown = true;
                Modules["Purchase Receipt Summaries"].DateFilter = true;
                Modules["Purchase Receipt Summaries"].DateFilterVisible = true;
                Modules["Purchase Receipt Summaries"].HasTotalRow = true;
            }
            {
                Modules.Add("Purchase Receipts For Vendor", new Module(
                    new List<GridColumn>() 
                    { 
                        new GridColumn("Invoice #"), 
                        new GridColumn("Invoice Line", typeof(Int32)), 
                        new GridColumn("Part"), 
                        new GridColumn("Description"), 
                        new GridColumn("Our Qty Received", typeof(Decimal)) 
                    }
                ));
                GridColumn gc = new GridColumn("Our Unit Cost", typeof(Decimal));
                gc.CellFormat = "c";
                Modules["Purchase Receipts For Vendor"].Columns.Add(gc);
                gc = new GridColumn("Our Total Cost", typeof(Decimal));
                gc.CellFormat = "c";
                Modules["Purchase Receipts For Vendor"].Columns.Add(gc);
                Modules["Purchase Receipts For Vendor"].Columns.AddRange(new List<GridColumn>() { new GridColumn("Job #"), new GridColumn("PO #"), new GridColumn("PO Line", typeof(Int32)), new GridColumn("PO Rel #", typeof(Int32)), new GridColumn("Received To"), new GridColumn("Receipt Date", typeof(DateTime)) });
                Modules["Purchase Receipts For Vendor"].DateFilter = true;
            }
            {
                Modules.Add("Purchase Receipt Summaries For G/L Account", new Module(new List<GridColumn>()));

                GridColumn gc = new GridColumn("G/L Account");
                gc.Visible = false;
                Modules["Purchase Receipt Summaries For G/L Account"].Columns.Add(gc);

                Modules["Purchase Receipt Summaries For G/L Account"].Columns.Add(new GridColumn("Vendor"));

                gc = new GridColumn("Total Receipt $");
                gc.CellFormat = "c";
                gc.DataType = typeof(Decimal);
                Modules["Purchase Receipt Summaries For G/L Account"].Columns.Add(gc);

                gc = new GridColumn("Vendor #");
                gc.Visible = false;
                Modules["Purchase Receipt Summaries For G/L Account"].Columns.Add(gc);

                Modules["Purchase Receipt Summaries For G/L Account"].AllowDrillDown = true;
                Modules["Purchase Receipt Summaries For G/L Account"].DateFilter = true;
            }

            #endregion

            #region Define Part Min Qty Alerts Module

            {
                Modules.Add("Part Min Qty Alerts", new Module(
                    new List<TCGEpicor.Module.ModuleAction>() 
                    { 
                        TCGEpicor.Module.ModuleAction.Favorite, 
                        TCGEpicor.Module.ModuleAction.Refresh, 
                        TCGEpicor.Module.ModuleAction.Save 
                    }
                ));
                Modules["Part Min Qty Alerts"].Columns.Add(new GridColumn("Part", true, true));
                Modules["Part Min Qty Alerts"].Columns.Add(new GridColumn("Description"));
                Modules["Part Min Qty Alerts"].Columns.Add(new GridColumn("Plant"));

                GridColumn gc = new GridColumn("On Hand Qty");
                gc.DataType = typeof(Decimal);
                Modules["Part Min Qty Alerts"].Columns.Add(gc);

                gc = new GridColumn("Avg Cost");
                gc.DataType = typeof(Decimal);
                gc.Visible = false;
                Modules["Part Min Qty Alerts"].Columns.Add(gc);

                gc = new GridColumn("Min Qty");
                gc.DataType = typeof(Decimal);
                Modules["Part Min Qty Alerts"].Columns.Add(gc);

                gc = new GridColumn("Difference");
                gc.DataType = typeof(Decimal);
                gc.CellFormat = "0%";
                Modules["Part Min Qty Alerts"].Columns.Add(gc);

                gc = new GridColumn("Monthly Usage");
                gc.DataType = typeof(Decimal);
                Modules["Part Min Qty Alerts"].Columns.Add(gc);

                gc = new GridColumn("Months To Stock", false);
                gc.DataType = typeof(Int32);
                Modules["Part Min Qty Alerts"].Columns.Add(gc);

                gc = new GridColumn("New Minimum", false);
                gc.DataType = typeof(Decimal);
                gc.UpdateTriggerColumns = new List<string>() { "Months To Stock" };
                gc.Function = new DataFunction("Months To Stock", "Monthly Usage");
                Modules["Part Min Qty Alerts"].Columns.Add(gc);

                gc = new GridColumn("Total Cost");
                gc.DataType = typeof(Decimal);
                gc.CellFormat = "c";
                gc.UpdateTriggerColumns = new List<string>() { "Months To Stock", "New Minimum" };
                gc.Function = new DataFunction("Avg Cost", "New Minimum");
                Modules["Part Min Qty Alerts"].Columns.Add(gc);

                gc = new GridColumn("Process", false);
                gc.ValidValues.Add("Yes");
                gc.ValidValues.Add("No");
                Modules["Part Min Qty Alerts"].Columns.Add(gc);

                DateTime timephase = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                while (timephase >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddYears(-1))
                {
                    gc = new GridColumn(timephase.Month.ToString() + "/" + timephase.Year.ToString(), true);
                    Modules["Part Min Qty Alerts"].Columns.Add(gc);
                    timephase = timephase.AddMonths(-1);
                }

                Modules["Part Min Qty Alerts"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
                Modules["Part Min Qty Alerts"].PreLoadDataCaptures.Add("Part Class", new Dictionary<string, string>());
            }

            #endregion

            #region Part Min Qty Report Module

            {
                Modules.Add("Part Min Qty Report", new Module(
                    new List<TCGEpicor.Module.ModuleAction>() 
                    { 
                        TCGEpicor.Module.ModuleAction.Favorite, 
                        TCGEpicor.Module.ModuleAction.Refresh
                    }
                ));
                Modules["Part Min Qty Report"].Columns.Add(new GridColumn("Part", true, true));
                Modules["Part Min Qty Report"].Columns.Add(new GridColumn("Description"));
                Modules["Part Min Qty Report"].Columns.Add(new GridColumn("Part Class"));
                Modules["Part Min Qty Report"].Columns.Add(new GridColumn("Plant"));

                GridColumn gc = new GridColumn("On Hand Qty");
                gc.DataType = typeof(Decimal);
                Modules["Part Min Qty Report"].Columns.Add(gc);

                gc = new GridColumn("Avg Cost");
                gc.DataType = typeof(Decimal);
                Modules["Part Min Qty Report"].Columns.Add(gc);

                gc = new GridColumn("Min Qty");
                gc.DataType = typeof(Decimal);
                Modules["Part Min Qty Report"].Columns.Add(gc);

                gc = new GridColumn("Difference");
                gc.DataType = typeof(Decimal);
                gc.CellFormat = "0%";
                Modules["Part Min Qty Report"].Columns.Add(gc);

                gc = new GridColumn("Monthly Usage");
                gc.DataType = typeof(Decimal);
                Modules["Part Min Qty Report"].Columns.Add(gc);

                DateTime timephase = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                while (timephase >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddYears(-1))
                {
                    gc = new GridColumn(timephase.Month.ToString() + "/" + timephase.Year.ToString(), true);
                    Modules["Part Min Qty Report"].Columns.Add(gc);
                    timephase = timephase.AddMonths(-1);
                }

                Modules["Part Min Qty Report"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
                Modules["Part Min Qty Report"].PreLoadDataCaptures.Add("Part Class", new Dictionary<string, string>());
            }

            #endregion

            #region Part Min Qty Template Module

            {
                Modules.Add("Part Min Qty Template", new Module(
                    new List<TCGEpicor.Module.ModuleAction>() 
                    { 
                        TCGEpicor.Module.ModuleAction.Favorite, 
                        TCGEpicor.Module.ModuleAction.Refresh
                    }
                ));
                Modules["Part Min Qty Template"].Columns.Add(new GridColumn("Part", true, true));
                Modules["Part Min Qty Template"].Columns.Add(new GridColumn("Description"));

                GridColumn gc = new GridColumn("On Hand Qty");
                gc.DataType = typeof(Decimal);
                Modules["Part Min Qty Template"].Columns.Add(gc);

                gc = new GridColumn("Unit Cost");
                gc.DataType = typeof(Decimal);
                gc.CellFormat = "0.00";
                Modules["Part Min Qty Template"].Columns.Add(gc);

                gc = new GridColumn("Count");
                Modules["Part Min Qty Template"].Columns.Add(gc);

                gc = new GridColumn("Adjust");
                Modules["Part Min Qty Template"].Columns.Add(gc);

                gc = new GridColumn("Dollars");
                Modules["Part Min Qty Template"].Columns.Add(gc);

                Modules["Part Min Qty Template"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
                Modules["Part Min Qty Template"].PreLoadDataCaptures.Add("Part Class", new Dictionary<string, string>());
            }

            #endregion

            #region JobMtls w/ IssuedQty 25% >= ReqQty - Ported

            Modules.Add("JobMtl w/ IssuedQty 25% >= ReqQty", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Job Status"),
                    new GridColumn("Created Date"),
                    new GridColumn("Req Due Date"),
                    new GridColumn("Plant"),
                    new GridColumn("Job #"),
                    new GridColumn("Job Part #"),
                    new GridColumn("Assembly"),
                    new GridColumn("Operation"),
                    new GridColumn("Mtl Part #"),
                    new GridColumn("Req Qty", typeof(Decimal)),
                    new GridColumn("Issued Qty", typeof(Decimal))
                }
            ));
            Modules["JobMtl w/ IssuedQty 25% >= ReqQty"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
            Modules["JobMtl w/ IssuedQty 25% >= ReqQty"].PreLoadDataCaptures.Add("Part Class", new Dictionary<string, string>());
            Modules["JobMtl w/ IssuedQty 25% >= ReqQty"].Filters.Add("Job Status", new Dictionary<string, string>() { { "All", "%" }, { "Open", "Open" }, { "Closed", "Closed" } });
            Modules["JobMtl w/ IssuedQty 25% >= ReqQty"].DateFilter = true;
            Modules["JobMtl w/ IssuedQty 25% >= ReqQty"].DateFilterVisible = true;

            #endregion

            #region Outsourced Materials Module

            Modules.Add("Outsourced Material Management", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Save,
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #"),
                    new GridColumn("Description"),
                    new GridColumn("Normally Purchased", false)
                }
            ));

            {
                GridColumn c = new GridColumn("Original Normally Purchased");
                c.Visible = false;
                Modules["Outsourced Material Management"].Columns.Add(c);
            }

            Modules["Outsourced Material Management"].Columns[2].ValidValues.Add("Yes");
            Modules["Outsourced Material Management"].Columns[2].ValidValues.Add("No");
            Modules["Outsourced Material Management"].Filters.Add("Normally Purchased", new Dictionary<string, string>() { { "Yes", "Yes" }, { "No", "No" } });
            Modules["Outsourced Material Management"].SelectedFilter["Normally Purchased"] = "No";

            #endregion

            #region Fuzzy Partnum Search

            Modules.Add("Fuzzy Partnum Search", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #"),
                    new GridColumn("Description"),
                    new GridColumn("Search Word"),
                    new GridColumn("Qty Bearing"),
                    new GridColumn("Type"),
                    new GridColumn("Phantom BOM"),
                    new GridColumn("Non-Stock Item"),
                    new GridColumn("Group"),
                    new GridColumn("Class"),
                    new GridColumn("Invty U/M"),
                    new GridColumn("InActive")
                }
            ));

            Modules["Fuzzy Partnum Search"].PreLoadDataCaptures.Add("Partnum", new Dictionary<string, string>());
            Modules["Fuzzy Partnum Search"].PreLoadDataCaptures["Partnum"] = new Dictionary<string, string>();

            #endregion

            #region Parts Over Ordered

            Modules.Add("Over Ordered Parts", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #"),
                    new GridColumn("Description"),
                    new GridColumn("On Hand Qty"),
                    new GridColumn("Max Qty"),
                    new GridColumn("Demand")
                }
            ));

            #endregion

            #region Zero Cost Parts

            Modules.Add("Zero Cost Parts", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #", true, true),
                    new GridColumn("Class"),
                    new GridColumn("Description"),
                    new GridColumn("On Hand"),
                    new GridColumn("Cost"),
                    new GridColumn("12mo Usage")
                }
                ));
            #endregion

            #region Part Time Phase

            Modules.Add("Part Time Phase", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #", true, true),
                    new GridColumn("Description"),
                    new GridColumn("Plant")
                }
            ));

            {
                DateTime timephase = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                while (timephase >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddYears(-1))
                {
                    GridColumn gc = new GridColumn(timephase.Month.ToString() + "/" + timephase.Year.ToString(), true);
                    Modules["Part Time Phase"].Columns.Add(gc);
                    timephase = timephase.AddMonths(-1);
                }

                Modules["Part Time Phase"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
                Modules["Part Time Phase"].PreLoadDataCaptures.Add("Part Class", new Dictionary<string, string>());
                Modules["Part Time Phase"].PreLoadDataCaptures.Add("PO Only", new Dictionary<string, string>() { {"No", "No"}, {"Yes", "Yes"} });
            }

            #endregion

            #region Part Time Phase

            Modules.Add("Part Time Phase w/ Suppliers", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part #", true, true),
                    new GridColumn("Description"),
                    new GridColumn("Plant"),
                    new GridColumn("Supplier"),
                    new GridColumn("On Hand Qty"),
                    new GridColumn("On Hand $"),
                    new GridColumn("Min Qty"),
                    new GridColumn("PO #"),
                    new GridColumn("PO Qty"),
                    new GridColumn("Due Date"),
                    new GridColumn("Avg Monthly")
                }
            ));

            {
                DateTime timephase = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                while (timephase >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddYears(-1))
                {
                    GridColumn gc = new GridColumn(timephase.Month.ToString() + "/" + timephase.Year.ToString(), true);
                    Modules["Part Time Phase w/ Suppliers"].Columns.Add(gc);
                    timephase = timephase.AddMonths(-1);
                }

                Modules["Part Time Phase w/ Suppliers"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
                Modules["Part Time Phase w/ Suppliers"].PreLoadDataCaptures.Add("Part Class", new Dictionary<string, string>());
            }

            #endregion

            #region New Count Group

            Modules.Add("New Count Group", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite
                },
                new List<GridColumn>()
                {
                    new GridColumn("Count Group"),
                    new GridColumn("Part")
                }
                ));
            Modules["New Count Group"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
            Modules["New Count Group"].PreLoadDataCaptures.Add("ABC Code", new Dictionary<string, string>());
            Modules["New Count Group"].PreLoadDataCaptures.Add("Limit", new Dictionary<string, string>());
            Modules["New Count Group"].PreLoadDataCaptures.Add("Bin From", new Dictionary<string, string>());
            Modules["New Count Group"].PreLoadDataCaptures.Add("Bin To", new Dictionary<string, string>());
            Modules["New Count Group"].PreLoadDataCaptures.Add("Fuzzy Part", new Dictionary<string, string>());
            Modules["New Count Group"].PreLoadDataCaptures.Add("Fuzzy Bin", new Dictionary<string, string>());
            Modules["New Count Group"].PreLoadDataCaptures.Add("Repair Date (mm/dd/yyyy)", new Dictionary<string, string>());

            #endregion

            #region Invoices

            Modules.Add("Invoices", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Plant"),
                    new GridColumn("Address 1"),
                    new GridColumn("Address 2"),
                    new GridColumn("City"),
                    new GridColumn("State"),
                    new GridColumn("Zip"),
                    new GridColumn("Invoice #", typeof(Int32)),
                    new GridColumn("Invoice Date", typeof(DateTime)),
                    new GridColumn("Order #", typeof(Int32)),
                    new GridColumn("Cust ID"),
                    new GridColumn("Customer"),
                    new GridColumn("PO #"),
                    new GridColumn("Part #"),
                    new GridColumn("Line Desc"),
                    new GridColumn("Doc Discount", typeof(Decimal)),
                    new GridColumn("Unit Price", typeof(Decimal)),
                    new GridColumn("Misc Code"),
                    new GridColumn("Misc Amt", typeof(Decimal)),
                    new GridColumn("Total Price", typeof(Decimal)),
                    new GridColumn("Shipment - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Invoice - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Labor Cost", typeof(Decimal)),
                    new GridColumn("Burden Cost", typeof(Decimal)),
                    new GridColumn("Material Cost", typeof(Decimal)),
                    new GridColumn("Subunit Cost", typeof(Decimal)),
                    new GridColumn("Total Cost", typeof(Decimal)),
                    new GridColumn("Margin"),
                    new GridColumn("Product Code"),
                    new GridColumn("Light Type"),
                    new GridColumn("Order Date", typeof(DateTime)),
                    new GridColumn("Need By Date", typeof(DateTime)),
                    new GridColumn("Promise Date", typeof(DateTime)),
                    new GridColumn("Sch Code"),
                    new GridColumn("Mark For"),
                    new GridColumn("Pack #"),
                    new GridColumn("Tracking #"),
                    new GridColumn("Ship Via")
                }
                ));
            Modules["Invoices"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Invoices"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Invoices"].PreLoadDataCaptures.Add("Prod Code", new Dictionary<string, string>());

            #endregion

            #region Invoices Extended

            Modules.Add("Invoices Extended", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Plant"),
                    new GridColumn("Address 1"),
                    new GridColumn("Address 2"),
                    new GridColumn("City"),
                    new GridColumn("State"),
                    new GridColumn("Zip"),
                    new GridColumn("Invoice #", typeof(Int32)),
                    new GridColumn("Invoice Date", typeof(DateTime)),
                    new GridColumn("Order #", typeof(Int32)),
                    new GridColumn("Cust ID"),
                    new GridColumn("Customer"),
                    new GridColumn("PO #"),
                    new GridColumn("Part #"),
                    new GridColumn("Line Desc"),
                    new GridColumn("Doc Discount", typeof(Decimal)),
                    new GridColumn("Unit Price", typeof(Decimal)),
                    new GridColumn("Misc Code"),
                    new GridColumn("Misc Amt", typeof(Decimal)),
                    new GridColumn("Total Price", typeof(Decimal)),
                    new GridColumn("Shipment - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Invoice - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Labor Cost", typeof(Decimal)),
                    new GridColumn("Burden Cost", typeof(Decimal)),
                    new GridColumn("Material Cost", typeof(Decimal)),
                    new GridColumn("Subunit Cost", typeof(Decimal)),
                    new GridColumn("Total Cost", typeof(Decimal)),
                    new GridColumn("Margin"),
                    new GridColumn("Product Code"),
                    new GridColumn("Construction Name"),
                    new GridColumn("Construction Code"),
                    new GridColumn("Num Doors"),
                    new GridColumn("Door Size"),
                    new GridColumn("Light Type"),
                    new GridColumn("LED Type"),
                    new GridColumn("Handle"),
                    new GridColumn("Finish"),
                    new GridColumn("Locks"),
                    new GridColumn("Full Silkscreen"),
                    new GridColumn("Back Kickplate"),
                    new GridColumn("Front Kickplate"),
                    new GridColumn("Back Bumperguard"),
                    new GridColumn("Front Bumperguard"),
                    new GridColumn("Inlay"),
                    new GridColumn("Order Date", typeof(DateTime)),
                    new GridColumn("Sch Code"),
                    new GridColumn("Mark For")
                }
                ));
            Modules["Invoices Extended"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Invoices Extended"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Invoices Extended"].PreLoadDataCaptures.Add("Part Class", new Dictionary<string, string>() { { "All", "%" }, { "ModLine", "ModLine" }, { "Classic", "Classic" }, { "20//20", "20//20" }, { "CAL-20", "CAL-20" }, { "Hybridoor", "Hybridoor" }, { "S//E", "S//E" } });

            #endregion

            #region Gross Margin

            Modules.Add("Gross Margin", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Invoice #", typeof(Int32)),
                    new GridColumn("Line #", typeof(Int32)),
                    new GridColumn("Plant"),
                    new GridColumn("Invoice Date", typeof(DateTime)),
                    new GridColumn("Order #", typeof(Int32)),
                    new GridColumn("Cust ID"),
                    new GridColumn("Customer"),
                    new GridColumn("PO #"),
                    new GridColumn("Part #"),
                    new GridColumn("Line Desc"),
                    new GridColumn("Doc Discount", typeof(Decimal)),
                    new GridColumn("Unit Price", typeof(Decimal)),
                    new GridColumn("Total Price", typeof(Decimal)),
                    new GridColumn("Shipment - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Invoice - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Labor Cost", typeof(Decimal)),
                    new GridColumn("Burden Cost", typeof(Decimal)),
                    new GridColumn("Burden Var", typeof(Decimal)),
                    new GridColumn("Material Cost", typeof(Decimal)),
                    new GridColumn("Material Var", typeof(Decimal)),
                    new GridColumn("Subunit Cost", typeof(Decimal)),
                    new GridColumn("Total Cost", typeof(Decimal)),
                    new GridColumn("Margin", typeof(Decimal)),
                    new GridColumn("Product Code")
                }
                ));
            Modules["Gross Margin"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
            Modules["Gross Margin"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Gross Margin"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Gross Margin"].PreLoadDataCaptures.Add("Burden Var %", new Dictionary<string, string>());
            Modules["Gross Margin"].PreLoadDataCaptures.Add("Material Var %", new Dictionary<string, string>());
            Modules["Gross Margin"].PreLoadDataCaptures.Add("Customer", new Dictionary<string, string>());

            #endregion

            #region Gross Margin w/ Freight

            Modules.Add("Gross Margin w/ Freight", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Invoice #", typeof(Int32)),
                    new GridColumn("Line #", typeof(Int32)),
                    new GridColumn("Plant"),
                    new GridColumn("Invoice Date", typeof(DateTime)),
                    new GridColumn("Order #", typeof(Int32)),
                    new GridColumn("Cust ID"),
                    new GridColumn("Customer"),
                    new GridColumn("PO #"),
                    new GridColumn("Part #"),
                    new GridColumn("Line Desc"),
                    new GridColumn("Doc Discount", typeof(Decimal)),
                    new GridColumn("Unit Price", typeof(Decimal)),
                    new GridColumn("Total Price", typeof(Decimal)),
                    new GridColumn("Shipment - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Invoice - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Labor Cost", typeof(Decimal)),
                    new GridColumn("Burden Cost", typeof(Decimal)),
                    new GridColumn("Burden Var", typeof(Decimal)),
                    new GridColumn("Material Cost", typeof(Decimal)),
                    new GridColumn("Material Var", typeof(Decimal)),
                    new GridColumn("Subunit Cost", typeof(Decimal)),
                    new GridColumn("Total Cost", typeof(Decimal)),
                    new GridColumn("Margin", typeof(Decimal)),
                    new GridColumn("Product Code")
                }
                ));
            Modules["Gross Margin w/ Freight"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
            Modules["Gross Margin w/ Freight"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Gross Margin w/ Freight"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Gross Margin w/ Freight"].PreLoadDataCaptures.Add("Burden Var %", new Dictionary<string, string>());
            Modules["Gross Margin w/ Freight"].PreLoadDataCaptures.Add("Material Var %", new Dictionary<string, string>());
            Modules["Gross Margin w/ Freight"].PreLoadDataCaptures.Add("Customer", new Dictionary<string, string>());

            #endregion

            #region Gross Margin w/ Rep Com

            Modules.Add("Gross Margin w/ Rep Com", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Invoice #", typeof(Int32)),
                    new GridColumn("Plant"),
                    new GridColumn("Invoice Date", typeof(DateTime)),
                    new GridColumn("Order #", typeof(Int32)),
                    new GridColumn("Cust ID"),
                    new GridColumn("Customer"),
                    new GridColumn("Sales Rep"),
                    new GridColumn("PO #"),
                    new GridColumn("Part #"),
                    new GridColumn("Line Desc"),
                    new GridColumn("Doc Discount", typeof(Decimal)),
                    new GridColumn("Unit Price", typeof(Decimal)),
                    new GridColumn("Total Price", typeof(Decimal)),
                    new GridColumn("Shipment - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Invoice - Our Ship Qty", typeof(Decimal)),
                    new GridColumn("Labor Cost", typeof(Decimal)),
                    new GridColumn("Burden Cost", typeof(Decimal)),
                    new GridColumn("Burden Var", typeof(Decimal)),
                    new GridColumn("Material Cost", typeof(Decimal)),
                    new GridColumn("Material Var", typeof(Decimal)),
                    new GridColumn("Subunit Cost", typeof(Decimal)),
                    new GridColumn("Noncomissionable", typeof(Decimal)),
                    new GridColumn("Comissionable", typeof(Decimal)),
                    new GridColumn("Rate", typeof(Decimal)),
                    new GridColumn("Comission", typeof(Decimal)),
                    new GridColumn("Total Cost", typeof(Decimal)),
                    new GridColumn("Margin", typeof(Decimal)),
                    new GridColumn("Product Code"),
                    new GridColumn("Order Line"),
                    new GridColumn("System"),
                    new GridColumn("Size"),
                    new GridColumn("Finish"),
                    new GridColumn("Light"),
                    new GridColumn("Lights"),
                    new GridColumn("Locks"),
                    new GridColumn("Handle"),
                    new GridColumn("Openings"),
                    new GridColumn("Width"),
                    new GridColumn("Height"),
                    new GridColumn("Area")
                }
                ));
            Modules["Gross Margin w/ Rep Com"].PreLoadDataCaptures.Add("Plant", new Dictionary<string, string>());
            Modules["Gross Margin w/ Rep Com"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Gross Margin w/ Rep Com"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Gross Margin w/ Rep Com"].PreLoadDataCaptures.Add("Burden Var %", new Dictionary<string, string>());
            Modules["Gross Margin w/ Rep Com"].PreLoadDataCaptures.Add("Material Var %", new Dictionary<string, string>());
            Modules["Gross Margin w/ Rep Com"].PreLoadDataCaptures.Add("Customer", new Dictionary<string, string>());

            #endregion

            #region Job Operation Metrics

            Modules.Add("Job Operation Metrics", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Job #"),
                    new GridColumn("Date", typeof(DateTime)),
                    new GridColumn("Part #"),
                    new GridColumn("Opr Seq"),
                    new GridColumn("Opr Code"),
                    new GridColumn("Qty", typeof(Decimal)),
                    new GridColumn("Hours", typeof(Decimal)),
                    new GridColumn("Avg", typeof(Decimal))
                }
                ));

            Modules["Job Operation Metrics"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Job Operation Metrics"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());

            #endregion

            #region Shipment Notifications

            Modules.Add("Shipment Notifications", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Cust ID"),
                    new GridColumn("Pro #"),
                    new GridColumn("Order #"),
                    new GridColumn("Pack #"),
                    new GridColumn("Sent"),
                    new GridColumn("Recipient")
                }
                ));
            {
                GridColumn gc = new GridColumn("Resend");
                gc.ActionColumn = true;
                Modules["Shipment Notifications"].Columns.Add(gc);
            }

            Modules["Shipment Notifications"].PreLoadDataCaptures.Add("Cust ID", new Dictionary<string, string>());
            Modules["Shipment Notifications"].PreLoadDataCaptures.Add("Order #", new Dictionary<string, string>());
            Modules["Shipment Notifications"].PreLoadDataCaptures.Add("Pack #", new Dictionary<string, string>());
            Modules["Shipment Notifications"].PreLoadDataCaptures.Add("Sent Date (MM/DD/YYYY)", new Dictionary<string, string>());

            #endregion

            #region Send Shipment Notification

            Modules.Add("Send Shipment Notification", new Module(
                new List<Module.ModuleAction>()
                {
                    Module.ModuleAction.Favorite
                },
                new List<GridColumn>()
                {
                }));

            Modules["Send Shipment Notification"].PreLoadDataCaptures.Add("Pack #", new Dictionary<string, string>());
            #endregion


            #region Power Analyzer Data

            Modules.Add("Power Analyzer Data", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Save,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Date"),
                    new GridColumn("Order #"),
                    new GridColumn("Job #"),
                    new GridColumn("Assembly #"),
                    new GridColumn("Assembly Rec"),
                    new GridColumn("Type"),
                    new GridColumn("Voltage"),
                    new GridColumn("Wattage"),
                    new GridColumn("Pf"),
                    new GridColumn("Ohms"),
                    new GridColumn("Amps")
                }));
            Modules["Power Analyzer Data"].PreLoadDataCaptures.Add("Order #", new Dictionary<string, string>());
            Modules["Power Analyzer Data"].PreLoadDataCaptures.Add("Entry Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Power Analyzer Data"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Power Analyzer Data"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Power Analyzer Data"].PreLoadDataCaptures.Add("Customer ('like' search)", new Dictionary<string, string>());
            Modules["Power Analyzer Data"].PreLoadDataCaptures.Add("Cust ID", new Dictionary<string, string>());

            #endregion

            #region Rails Quote Data

            Modules.Add("Rails Quote Data", new Module(
                            new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                            new List<GridColumn>()
                {
                    new GridColumn("Cust #"),
                    new GridColumn("Customer"),
                    new GridColumn("Job Ref"),
                    new GridColumn("Contact"),
                    new GridColumn("Sales Rep"),
                    new GridColumn("CSR"),
                    new GridColumn("Quote Date"),
                    new GridColumn("Quote #"),
                    new GridColumn("Vantage Date"),
                    new GridColumn("Vantage Quote #"),
                    new GridColumn("Convert Days", typeof(String)),
                    new GridColumn("Quote Subtotal"),
                    new GridColumn("Line #"),
                    new GridColumn("Part"),
                    new GridColumn("Group"),
                    new GridColumn("Description"),
                    new GridColumn("Qty"),
                    new GridColumn("Price"),
                    new GridColumn("Discount %"),
                    new GridColumn("Adj Amt"),
                    new GridColumn("Adj Code"),
                    new GridColumn("Act Ship"),
                    new GridColumn("Promise Date"),
                    new GridColumn("Ship Date")
                }));
            Modules["Rails Quote Data"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Rails Quote Data"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());
            #endregion

            #region Rails Quote Summary

            Modules.Add("Rails Quote Summary", new Module(new List<TCGEpicor.Module.ModuleAction>()
            {
                TCGEpicor.Module.ModuleAction.Favorite,
                TCGEpicor.Module.ModuleAction.Refresh
            },
            new List<GridColumn>()
            {
                new GridColumn("Group"),
                new GridColumn("Qty"),
                new GridColumn("Quote $"),
                new GridColumn("Order $"),
                new GridColumn("Conversion")
            }));
            Modules["Rails Quote Summary"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Rails Quote Summary"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["Rails Quote Summary"].PreLoadDataCaptures.Add("By", new Dictionary<string, string> { { "Week", "Week" }, { "Product", "Product" } });

            #endregion

            #region WBM Data

            Modules.Add("WBM CS/ENG Usage Report", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("Message #"),
                    new GridColumn("Message Type"),
                    new GridColumn("CS Steps"),
                    new GridColumn("CS Time"),
                    new GridColumn("Eng Steps"),
                    new GridColumn("Eng Time")
                }
                ));
            Modules["WBM CS/ENG Usage Report"].AllowDrillDown = true;
            Modules["WBM CS/ENG Usage Report"].PreLoadDataCaptures.Add("Start Date (MM/DD/YYYY)", new Dictionary<string, string>());
            Modules["WBM CS/ENG Usage Report"].PreLoadDataCaptures.Add("End Date (MM/DD/YYYY)", new Dictionary<string, string>());

            Modules.Add("Message Details", new Module(
                new List<GridColumn>()
                {
                    new GridColumn("Dept"),
                    new GridColumn("Step Name"),
                    new GridColumn("User"),
                    new GridColumn("Wait Time")
                }
                ));
            #endregion

            #region Customer Export

            Modules.Add("Customer Export", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite
                },
                new List<GridColumn>()
                {
                    new GridColumn("Cust ID"),
                    new GridColumn("Name"),
                    new GridColumn("Type"),
                    new GridColumn("Address 1"),
                    new GridColumn("Address 2"),
                    new GridColumn("Address 3"),
                    new GridColumn("City"),
                    new GridColumn("State"),
                    new GridColumn("Zip"),
                    new GridColumn("Country"),
                    new GridColumn("Sales Rep"),
                    new GridColumn("Phone #"),
                    new GridColumn("Fax #"),
                    new GridColumn("Email"),
                    new GridColumn("URL"),
                    new GridColumn("Established"),
                    new GridColumn("Discount %"),
                    new GridColumn("Bill To"),
                    new GridColumn("Bill To Address 1"),
                    new GridColumn("Bill To Address 2"),
                    new GridColumn("Bill To Address 3"),
                    new GridColumn("Bill To City"),
                    new GridColumn("Bill To State"),
                    new GridColumn("Bill To Zip"),
                    new GridColumn("Bill To Phone"),
                    new GridColumn("Bill To Fax")
                }
                ));
            #endregion

            #region Forecast Import

            Modules.Add("Forecast Import", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite
                },
                new List<GridColumn>()
                {
                    new GridColumn("Part"),
                    new GridColumn("Plant"),
                    new GridColumn("Date"),
                    new GridColumn("Qty"),
                    new GridColumn("Customer")
                }
                ));

            Modules["Forecast Import"].PreLoadDataCaptures.Add("Import File", new Dictionary<string, string>() { { "FILE", "FILE" } });

            #endregion

            #region Job Mtl Replacement

            Modules.Add("Batch Material Replacement", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh,
                    TCGEpicor.Module.ModuleAction.Save
                },
                new List<GridColumn>()
                {
                    new GridColumn("Job #"),
                    new GridColumn("Job Part"),
                    new GridColumn("Assembly Seq"),
                    new GridColumn("Asm Part"),
                    new GridColumn("Opr Seq"),
                    new GridColumn("Op Code"),
                    new GridColumn("Mtl Seq"),
                    new GridColumn("Qty"),
                    new GridColumn("Part #"),
                    new GridColumn("MES"),
                    new GridColumn("New Part #"),
                    new GridColumn("New MES"),
                    new GridColumn("New Qty Per")
                }
                ));
            {
                GridColumn gc = new GridColumn("Confirm");
                gc.ReadOnly = false;
                gc.ValidValues = new List<string>() { "Yes", "No" };
                Modules["Batch Material Replacement"].Columns.Add(gc);
            }

            Modules["Batch Material Replacement"].PreLoadDataCaptures.Add("Original Part #", new Dictionary<string, string>());
            Modules["Batch Material Replacement"].PreLoadDataCaptures.Add("Original MES", new Dictionary<string, string>());
            Modules["Batch Material Replacement"].PreLoadDataCaptures.Add("New Part # (blank for original)", new Dictionary<string, string>());
            Modules["Batch Material Replacement"].PreLoadDataCaptures.Add("New MES (blank for original)", new Dictionary<string, string>());
            Modules["Batch Material Replacement"].PreLoadDataCaptures.Add("New Qty Per (blank for original)", new Dictionary<string, string>());

            #endregion

            #region LEGACY - Sales Orders

            Modules.Add("Sales Orders", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {
                    TCGEpicor.Module.ModuleAction.Favorite,
                    TCGEpicor.Module.ModuleAction.Refresh
                },
                new List<GridColumn>()
                {
                    new GridColumn("OpenOrder"),
                    new GridColumn("VoidOrder"),
                    new GridColumn("Company"),
                    new GridColumn("OrderNum"),
                    new GridColumn("PONum"),
                    new GridColumn("OrderHeld"),
                    new GridColumn("EntryPerson"),
                    new GridColumn("ShipToNum"),
                    new GridColumn("RequestDate"),
                    new GridColumn("OrderDate"),
                    new GridColumn("FOB"),
                    new GridColumn("ShipViaCode"),
                    new GridColumn("TermsCode"),
                    new GridColumn("DiscountPercent"),
                    new GridColumn("PrcConNum"),
                    new GridColumn("ShpConNum"),
                    new GridColumn("SalesRepList"),
                    new GridColumn("OrderComment"),
                    new GridColumn("ShipComment"),
                    new GridColumn("InvoiceComment"),
                    new GridColumn("PickListComment"),
                    new GridColumn("DepositBal"),
                    new GridColumn("DocDepositBal"),
                    new GridColumn("NeedByDate"),
                    new GridColumn("CreditOverride"),
                    new GridColumn("CreditOverrideUserID"),
                    new GridColumn("CreditOverrideLimit"),
                    new GridColumn("SndAlrtShp"),
                    new GridColumn("ExchangeRate"),
                    new GridColumn("CurrencyCode"),
                    new GridColumn("LockRate"),
                    new GridColumn("CardMemberName"),
                    new GridColumn("CardNumber"),
                    new GridColumn("CardType"),
                    new GridColumn("ExpirationMonth"),
                    new GridColumn("ExpirationYear"),
                    new GridColumn("CardID"),
                    new GridColumn("CardmemberReference"),
                    new GridColumn("AllocPriorityCode"),
                    new GridColumn("ShipOrderComplete"),
                    new GridColumn("EDIOrder"),
                    new GridColumn("EDIAck"),
                    new GridColumn("Linked"),
                    new GridColumn("ICPONum"),
                    new GridColumn("ExtCompany"),
                    new GridColumn("AckEmailSent"),
                    new GridColumn("ApplyOrderBasedDisc"),
                    new GridColumn("EntryMethod"),
                    new GridColumn("CounterSale"),
                    new GridColumn("CreateInvoice"),
                    new GridColumn("CreatePackingSlip"),
                    new GridColumn("LockQty"),
                    new GridColumn("ProcessCard"),
                    new GridColumn("CCAmount"),
                    new GridColumn("CCFreight"),
                    new GridColumn("CCTax"),
                    new GridColumn("CCDocAmount"),
                    new GridColumn("CCDocFreight"),
                    new GridColumn("CCDocTax"),
                    new GridColumn("CCStreetAddr"),
                    new GridColumn("CCZip"),
                    new GridColumn("BTCustNum"),
                    new GridColumn("BTConNum"),
                    new GridColumn("RepRate4"),
                    new GridColumn("RepRate5"),
                    new GridColumn("RepSplit1"),
                    new GridColumn("RepSplit2"),
                    new GridColumn("RepSplit3"),
                    new GridColumn("RepSplit4"),
                    new GridColumn("RepSplit5"),
                    new GridColumn("RepRate1"),
                    new GridColumn("RepRate2"),
                    new GridColumn("RepRate3"),
                    new GridColumn("OutboundSalesDocCtr"),
                    new GridColumn("OutboundShipDocsCtr"),
                    new GridColumn("DemandContractNum"),
                    new GridColumn("DoNotShipBeforeDate"),
                    new GridColumn("ResDelivery"),
                    new GridColumn("DoNotShipAfterDate"),
                    new GridColumn("SatDelivery"),
                    new GridColumn("SatPickup"),
                    new GridColumn("Hazmat"),
                    new GridColumn("DocOnly"),
                    new GridColumn("RefNotes"),
                    new GridColumn("ApplyChrg"),
                    new GridColumn("ChrgAmount"),
                    new GridColumn("COD"),
                    new GridColumn("CODFreight"),
                    new GridColumn("CODCheck"),
                    new GridColumn("CODAmount"),
                    new GridColumn("GroundType"),
                    new GridColumn("NotifyFlag"),
                    new GridColumn("NotifyEMail"),
                    new GridColumn("DeclaredIns"),
                    new GridColumn("DeclaredAmt"),
                    new GridColumn("CancelAfterDate"),
                    new GridColumn("DemandRejected"),
                    new GridColumn("OverrideCarrier"),
                    new GridColumn("OverrideService"),
                    new GridColumn("CreditCardOrder"),
                    new GridColumn("DemandHeadSeq"),
                    new GridColumn("PayFlag"),
                    new GridColumn("PayAccount"),
                    new GridColumn("PayBTAddress1"),
                    new GridColumn("PayBTAddress2"),
                    new GridColumn("PayBTCity"),
                    new GridColumn("PayBTState"),
                    new GridColumn("PayBTZip"),
                    new GridColumn("PayBTCountry"),
                    new GridColumn("DropShip"),
                    new GridColumn("CommercialInvoice"),
                    new GridColumn("ShipExprtDeclartn"),
                    new GridColumn("CertOfOrigin"),
                    new GridColumn("LetterOfInstr"),
                    new GridColumn("FFID"),
                    new GridColumn("FFAddress1"),
                    new GridColumn("FFAddress2"),
                    new GridColumn("FFCity"),
                    new GridColumn("FFState"),
                    new GridColumn("FFZip"),
                    new GridColumn("FFCountry"),
                    new GridColumn("FFContact"),
                    new GridColumn("FFCompName"),
                    new GridColumn("FFPhoneNum"),
                    new GridColumn("IntrntlShip"),
                    new GridColumn("AutoPrintReady"),
                    new GridColumn("EDIReady"),
                    new GridColumn("IndividualPackIDs"),
                    new GridColumn("FFAddress3"),
                    new GridColumn("DeliveryConf"),
                    new GridColumn("AddlHdlgFlag"),
                    new GridColumn("NonStdPkg"),
                    new GridColumn("ServSignature"),
                    new GridColumn("ServAlert"),
                    new GridColumn("ServHomeDel"),
                    new GridColumn("DeliveryType"),
                    new GridColumn("FFCountryNum"),
                    new GridColumn("PayBTAddress3"),
                    new GridColumn("PayBTCountryNum"),
                    new GridColumn("PayBTPhone"),
                    new GridColumn("ReadyToCalc"),
                    new GridColumn("BTCustNumCustID"),
                    new GridColumn("CustomerCustID"),
                    new GridColumn("promisedate"),
                    new GridColumn("schcode"),
                    new GridColumn("frtterms"),
                    new GridColumn("bolnotes"),
                    new GridColumn("actship"),
                    new GridColumn("actshipweight"),
                    new GridColumn("thirdpartybill"),
                    new GridColumn("markfor"),
                    new GridColumn("actshipcharge")
                }
                ));

            Modules["Sales Orders"].AllowDrillDown = true;
            Modules["Sales Orders"].PreLoadDataCaptures.Add("Order #", new Dictionary<string, string>());

            Modules.Add("Order Lines", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {

                },
                new List<GridColumn>()
                {
                    new GridColumn("Image", typeof(Image)),
                    new GridColumn("VoidLine"),
                    new GridColumn("OpenLine"),
                    new GridColumn("Company"),
                    new GridColumn("OrderNum"),
                    new GridColumn("OrderLine"),
                    new GridColumn("LineType"),
                    new GridColumn("PartNum"),
                    new GridColumn("LineDesc"),
                    new GridColumn("Reference"),
                    new GridColumn("IUM"),
                    new GridColumn("RevisionNum"),
                    new GridColumn("POLine"),
                    new GridColumn("Commissionable"),
                    new GridColumn("DiscountPercent"),
                    new GridColumn("UnitPrice"),
                    new GridColumn("DocUnitPrice"),
                    new GridColumn("OrderQty"),
                    new GridColumn("Discount"),
                    new GridColumn("DocDiscount"),
                    new GridColumn("RequestDate"),
                    new GridColumn("ProdCode"),
                    new GridColumn("XPartNum"),
                    new GridColumn("XRevisionNum"),
                    new GridColumn("PricePerCode"),
                    new GridColumn("OrderComment"),
                    new GridColumn("ShipComment"),
                    new GridColumn("InvoiceComment"),
                    new GridColumn("PickListComment"),
                    new GridColumn("TaxCatID"),
                    new GridColumn("AdvanceBillBal"),
                    new GridColumn("DocAdvanceBillBal"),
                    new GridColumn("QuoteNum"),
                    new GridColumn("QuoteLine"),
                    new GridColumn("TMBilling"),
                    new GridColumn("OrigWhyNoTax"),
                    new GridColumn("NeedByDate"),
                    new GridColumn("Rework"),
                    new GridColumn("RMANum"),
                    new GridColumn("RMALine"),
                    new GridColumn("ProjectID"),
                    new GridColumn("ContractNum"),
                    new GridColumn("ContractCode"),
                    new GridColumn("Warranty"),
                    new GridColumn("WarrantyCode"),
                    new GridColumn("MaterialDuration"),
                    new GridColumn("LaborDuration"),
                    new GridColumn("MiscDuration"),
                    new GridColumn("MaterialMod"),
                    new GridColumn("LaborMod"),
                    new GridColumn("WarrantyComment"),
                    new GridColumn("Onsite"),
                    new GridColumn("MatCovered"),
                    new GridColumn("LabCovered"),
                    new GridColumn("MiscCovered"),
                    new GridColumn("SalesUM"),
                    new GridColumn("SellingFactor"),
                    new GridColumn("SellingQuantity"),
                    new GridColumn("SalesCatID"),
                    new GridColumn("ShipLineComplete"),
                    new GridColumn("CumeQty"),
                    new GridColumn("CumeDate"),
                    new GridColumn("MktgCampaignID"),
                    new GridColumn("MktgEvntSeq"),
                    new GridColumn("LockQty"),
                    new GridColumn("Linked"),
                    new GridColumn("ICPONum"),
                    new GridColumn("ICPOLine"),
                    new GridColumn("ExtCompany"),
                    new GridColumn("LastConfigDate"),
                    new GridColumn("LastConfigTime"),
                    new GridColumn("LastConfigUserID"),
                    new GridColumn("ConfigUnitPrice"),
                    new GridColumn("ConfigBaseUnitPrice"),
                    new GridColumn("PriceListCode"),
                    new GridColumn("BreakListCode"),
                    new GridColumn("LockPrice"),
                    new GridColumn("ListPrice"),
                    new GridColumn("DocListPrice"),
                    new GridColumn("OrdBasedPrice"),
                    new GridColumn("DocOrdBasedPrice"),
                    new GridColumn("PriceGroupCode"),
                    new GridColumn("OverridePriceList"),
                    new GridColumn("PricingValue"),
                    new GridColumn("DisplaySeq"),
                    new GridColumn("KitParentLine"),
                    new GridColumn("KitAllowUpdate"),
                    new GridColumn("KitShipComplete"),
                    new GridColumn("KitBackFlush"),
                    new GridColumn("KitPrintCompsPS"),
                    new GridColumn("KitPrintCompsInv"),
                    new GridColumn("KitPricing"),
                    new GridColumn("KitQtyPer"),
                    new GridColumn("SellingFactorDirection"),
                    new GridColumn("RepRate1"),
                    new GridColumn("RepRate2"),
                    new GridColumn("RepRate3"),
                    new GridColumn("RepRate4"),
                    new GridColumn("RepRate5"),
                    new GridColumn("RepSplit1"),
                    new GridColumn("RepSplit2"),
                    new GridColumn("RepSplit3"),
                    new GridColumn("RepSplit4"),
                    new GridColumn("RepSplit5"),
                    new GridColumn("DemandContractLine"),
                    new GridColumn("CreateNewJob"),
                    new GridColumn("DoNotShipBeforeDate"),
                    new GridColumn("GetDtls"),
                    new GridColumn("DoNotShipAfterDate"),
                    new GridColumn("SchedJob"),
                    new GridColumn("RelJob"),
                    new GridColumn("EnableCreateNewJob"),
                    new GridColumn("EnableGetDtls"),
                    new GridColumn("EnableSchedJob"),
                    new GridColumn("EnableRelJob"),
                    new GridColumn("CounterSaleWarehouse"),
                    new GridColumn("CounterSaleBinNum"),
                    new GridColumn("CounterSaleLotNum"),
                    new GridColumn("CounterSaleDimCode"),
                    new GridColumn("DemandDtlRejected"),
                    new GridColumn("KitFlag"),
                    new GridColumn("KitsLoaded"),
                    new GridColumn("DemandContractNum"),
                    new GridColumn("DemandHeadSeq"),
                    new GridColumn("DemandDtlSeq"),
                    new GridColumn("ReverseCharge"),
                    new GridColumn("CustNumCustID"),
                    new GridColumn("SAP #")
                }));

            Modules.Add("Order Memos", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {

                },
                new List<GridColumn>()
                {
                    new GridColumn("company"),
                    new GridColumn("ordernum"),
                    new GridColumn("orderline"),
                    new GridColumn("orderrelnum"),
                    new GridColumn("memodate"),
                    new GridColumn("memonum"),
                    new GridColumn("memouserid"),
                    new GridColumn("notify"),
                    new GridColumn("notifyuserid"),
                    new GridColumn("notifydate"),
                    new GridColumn("memodesc"),
                    new GridColumn("memotext"),
                    new GridColumn("categoryid")
                }
                ));

            Modules.Add("Order Misc Charges", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {

                },
                new List<GridColumn>()
                {
                    new GridColumn("Company"),
                    new GridColumn("OrderNum"),
                    new GridColumn("OrderLine"),
                    new GridColumn("SeqNum"),
                    new GridColumn("MiscCode"),
                    new GridColumn("Description"),
                    new GridColumn("MiscAmt"),
                    new GridColumn("DocMiscAmt"),
                    new GridColumn("FreqCode"),
                    new GridColumn("Quoting"),
                    new GridColumn("Linked"),
                    new GridColumn("ICPONum"),
                    new GridColumn("ICPOLine"),
                    new GridColumn("ICPOSeqNum"),
                    new GridColumn("ExtCompany")
                }
                ));

            Modules.Add("Order Releases", new Module(
                new List<TCGEpicor.Module.ModuleAction>()
                {

                },
                new List<GridColumn>()
                {
                    new GridColumn("Company"),
                    new GridColumn("OrderNum"),
                    new GridColumn("OrderLine"),
                    new GridColumn("OrderRelNum"),
                    new GridColumn("LineType"),
                    new GridColumn("ReqDate"),
                    new GridColumn("OurReqQty"),
                    new GridColumn("ShipToNum"),
                    new GridColumn("ShipViaCode"),
                    new GridColumn("OpenRelease"),
                    new GridColumn("FirmRelease"),
                    new GridColumn("Make"),
                    new GridColumn("OurJobQty"),
                    new GridColumn("OurJobShippedQty"),
                    new GridColumn("VoidRelease"),
                    new GridColumn("OurStockQty"),
                    new GridColumn("WarehouseCode"),
                    new GridColumn("OurStockShippedQty"),
                    new GridColumn("PartNum"),
                    new GridColumn("RevisionNum"),
                    new GridColumn("TaxExempt"),
                    new GridColumn("ShpConNum"),
                    new GridColumn("NeedByDate"),
                    new GridColumn("Reference"),
                    new GridColumn("Plant"),
                    new GridColumn("SellingReqQty"),
                    new GridColumn("SellingJobQty"),
                    new GridColumn("SellingJobShippedQty"),
                    new GridColumn("SellingStockQty"),
                    new GridColumn("SellingStockShippedQty"),
                    new GridColumn("SelectForPicking"),
                    new GridColumn("StagingWarehouseCode"),
                    new GridColumn("StagingBinNum"),
                    new GridColumn("PickError"),
                    new GridColumn("CumeQty"),
                    new GridColumn("CumeDate"),
                    new GridColumn("Linked"),
                    new GridColumn("ICPONum"),
                    new GridColumn("ICPOLine"),
                    new GridColumn("ICPORelNum"),
                    new GridColumn("ExtCompany"),
                    new GridColumn("ScheduleNumber"),
                    new GridColumn("MarkForNum"),
                    new GridColumn("DropShipName"),
                    new GridColumn("RAN"),
                    new GridColumn("DemandReference"),
                    new GridColumn("DemandSchedRejected"),
                    new GridColumn("DatePickTicketPrinted"),
                    new GridColumn("ResDelivery"),
                    new GridColumn("SatDelivery"),
                    new GridColumn("SatPickup"),
                    new GridColumn("VerbalConf"),
                    new GridColumn("Hazmat"),
                    new GridColumn("DocOnly"),
                    new GridColumn("RefNotes"),
                    new GridColumn("ApplyChrg"),
                    new GridColumn("ChrgAmount"),
                    new GridColumn("COD"),
                    new GridColumn("CODFreight"),
                    new GridColumn("CODCheck"),
                    new GridColumn("CODAmount"),
                    new GridColumn("GroundType"),
                    new GridColumn("NotifyFlag"),
                    new GridColumn("NotifyEMail"),
                    new GridColumn("DeclaredIns"),
                    new GridColumn("DeclaredAmt"),
                    new GridColumn("ServSatDelivery"),
                    new GridColumn("ServSatPickup"),
                    new GridColumn("ServSignature"),
                    new GridColumn("ServAlert"),
                    new GridColumn("ServPOD"),
                    new GridColumn("ServAOD"),
                    new GridColumn("ServHomeDel"),
                    new GridColumn("DeliveryType"),
                    new GridColumn("ServDeliveryDate"),
                    new GridColumn("ServPhone"),
                    new GridColumn("ServInstruct"),
                    new GridColumn("ServRelease"),
                    new GridColumn("ServAuthNum"),
                    new GridColumn("ServRef1"),
                    new GridColumn("ServRef2"),
                    new GridColumn("ServRef3"),
                    new GridColumn("ServRef4"),
                    new GridColumn("ServRef5"),
                    new GridColumn("OverrideCarrier"),
                    new GridColumn("OverrideService"),
                    new GridColumn("DockingStation"),
                    new GridColumn("Location"),
                    new GridColumn("TransportID"),
                    new GridColumn("ShipbyTime"),
                    new GridColumn("TaxConnectCalc"),
                    new GridColumn("GetDfltTaxIds")
                }
                ));

            #endregion


        }

        #endregion

        /* Add security stuff in Helper Methods - SecureMenu */
        /* Add module method call to LoadData */
        /* Add data capture selections to PopulateDataCaptures */
        #region Private Methods

        /* Add security stuff in SecureMenu */
        #region Helper Methods

        private DataTable GenerateGridTable(List<GridColumn> columns)
        {
            DataTable dt = new DataTable();
            foreach (GridColumn column in columns)
            {
                System.Data.DataColumn c = new DataColumn();
                c.ColumnName = column.Name;
                c.ReadOnly = column.ReadOnly;
                c.DataType = column.DataType;
                dt.Columns.Add(c);
            }
            return dt;
        }

        private string GetKeyValue(string key)
        {
            IsolatedStorageFileStream isoStream = null;
            IsolatedStorageFile isoStore = null;
            try
            {
                isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming, null, null);
                if (isoStore.FileExists("TCGEpicor" + key + ".txt"))
                {
                    isoStream = new IsolatedStorageFileStream("TCGEpicor" + key + ".txt", FileMode.Open, isoStore);
                    StreamReader reader = new StreamReader(isoStream);
                    string value = reader.ReadLine();
                    reader.Close();
                    return value;
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("Problem loading your stored connection info.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return "";
        }

        private void SetKeyValue(string key, string value)
        {
            IsolatedStorageFileStream isoStream = null;
            IsolatedStorageFile isoStore = null;
            try
            {
                isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming, null, null);
                if (isoStore.FileExists("TCGEpicor" + key + ".txt"))
                    isoStream = new IsolatedStorageFileStream("TCGEpicor" + key + ".txt", FileMode.Open, isoStore);
                else
                    isoStream = new IsolatedStorageFileStream("TCGEpicor" + key + ".txt", FileMode.Create, isoStore);
                StreamWriter writer = new StreamWriter(isoStream);
                isoStream.Position = 0;
                writer.WriteLine(value);
                writer.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Problem saving your connection info.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ThreadSafeModify(Control control, controlMethod method)
        {
            if (control.InvokeRequired)
                control.Invoke(new MethodInvoker(method));
            else
                method();
        }

        private void FormatCells(DataGridView dgv, List<GridColumn> columns, string module)
        {
            foreach (GridColumn column in columns)
            {
                dgv.Columns[column.Name].DefaultCellStyle.WrapMode = DataGridViewTriState.True;                
                if (!String.IsNullOrEmpty(column.CellFormat))
                    dgv.Columns[column.Name].DefaultCellStyle.Format = column.CellFormat;
                if (column.Frozen)
                    dgv.Columns[column.Name].Frozen = true;
                if (!column.Visible)
                    dgv.Columns[column.Name].Visible = false;
                if (column.ValidValues.Count > 0)
                {
                    int index = dgv.Columns.IndexOf(dgv.Columns[column.Name]);
                    dgv.Columns.Remove(column.Name);
                    DataGridViewComboBoxColumn cbc = new DataGridViewComboBoxColumn();
                    cbc.Name = column.Name;
                    cbc.DataPropertyName = column.Name;
                    foreach (string key in column.ValidValues)
                        cbc.Items.Add(key);
                    dgv.Columns.Insert(index, cbc);
                }
                if (column.ActionColumn)
                {
                    dgv.Columns.Remove(column.Name);
                    DataGridViewLinkColumn c = new DataGridViewLinkColumn();
                    c.UseColumnTextForLinkValue = false;
                    c.HeaderText = column.Name;
                    c.Name = column.Name;
                    c.DataPropertyName = column.Name;
                    int index = columns.IndexOf(column);
                    if (dgv.Columns[0].Name == "")
                        index++;
                    dgv.Columns.Insert(index, c);
                }
                if (column.Image)
                {
                    dgv.Columns.Remove(column.Name);
                    DataGridViewImageColumn c = new DataGridViewImageColumn();
                    c.HeaderText = column.Name;
                    c.Name = column.Name;
                    c.DataPropertyName = column.Name;
                    
                    int index = columns.IndexOf(column);
                    if (dgv.Columns[0].Name == "")
                        index++;
                    dgv.Columns.Insert(index, c);
                }
            }
            if (module == "Get Jobs Missing Parts Details")
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (((DataRowView)row.DataBoundItem)["Below Req Qty"].ToString() == "Yes")
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    if (((DataRowView)row.DataBoundItem)["Backflush"].ToString() == "1")
                        row.DefaultCellStyle.BackColor = Color.LightBlue;

                }
            }
        }

        private void BuildDateFilter(string module)
        {
            if (Modules[module].DateFilterVisible)
            {
                ToolStripButton backWholeMonth = new ToolStripButton();
                backWholeMonth.Name = "backWholeMonth";
                backWholeMonth.Image = global::TCGEpicor.Properties.Resources._1206569967984704715pitr_green_double_arrows_set_4_svg_hi;
                backWholeMonth.Click += new EventHandler(backWholeMonth_Click);
                backWholeMonth.ToolTipText = "Previous whole month";

                ToolStripButton backMonth = new ToolStripButton();
                backMonth.Name = "backMonth";
                backMonth.Image = global::TCGEpicor.Properties.Resources._12065699261710976909pitr_green_single_arrows_set_4_svg_hi;
                backMonth.Click += new EventHandler(backMonth_Click);
                backMonth.ToolTipText = "Previous 30 days";

                ToolStripButton nextMonth = new ToolStripButton();
                nextMonth.Name = "nextMonth";
                nextMonth.Image = global::TCGEpicor.Properties.Resources._1206569902228245216pitr_green_single_arrows_set_1_svg_hi;
                nextMonth.Click += new EventHandler(nextMonth_Click);
                nextMonth.ToolTipText = "Next 30 days";

                ToolStripButton nextWholeMonth = new ToolStripButton();
                nextWholeMonth.Name = "nextWholeMonth";
                nextWholeMonth.Image = global::TCGEpicor.Properties.Resources._1206569942771180767pitr_green_double_arrows_set_1_svg_hi;
                nextWholeMonth.Click += new EventHandler(nextWholeMonth_Click);
                nextWholeMonth.ToolTipText = "Next whole month";

                ToolStripLabel fromLbl = new ToolStripLabel();
                fromLbl.Name = "fromLbl";
                fromLbl.Text = "From";

                ToolStripLabel toLbl = new ToolStripLabel();
                toLbl.Name = "toLbl";
                toLbl.Text = "To";

                DateTimePicker fromMonth = new DateTimePicker();
                fromMonth.Name = "fromMonth";
                fromMonth.Format = DateTimePickerFormat.Short;
                fromMonth.Width = 100;
                fromMonth.Text = Modules[module].From.ToShortDateString();
                fromMonth.TextChanged += new EventHandler(fromMonth_TextChanged);
                ToolStripControlHost fromtch = new ToolStripControlHost(fromMonth);
                fromtch.Name = "fromMonthTCH";

                DateTimePicker toMonth = new DateTimePicker();
                toMonth.Name = "toMonth";
                toMonth.Format = DateTimePickerFormat.Short;
                toMonth.Width = 100;
                toMonth.Text = Modules[module].To.ToShortDateString();
                toMonth.TextChanged += new EventHandler(toMonth_TextChanged);
                ToolStripControlHost totch = new ToolStripControlHost(toMonth);
                totch.Name = "toMonthTCH";

                toolStrip1.Items.Add(backWholeMonth);
                toolStrip1.Items.Add(backMonth);
                toolStrip1.Items.Add(fromLbl);
                ThreadSafeModify(toolStrip1, delegate { toolStrip1.Items.Add(fromtch); });
                toolStrip1.Items.Add(toLbl);
                ThreadSafeModify(toolStrip1, delegate { toolStrip1.Items.Add(totch); });
                toolStrip1.Items.Add(nextMonth);
                toolStrip1.Items.Add(nextWholeMonth);
            }
        }

        private void DestroyDateFilter()
        {
            toolStrip1.Items.RemoveByKey("backWholeMonth");
            toolStrip1.Items.RemoveByKey("backMonth");
            toolStrip1.Items.RemoveByKey("fromLbl");
            toolStrip1.Items.RemoveByKey("fromMonthTCH");
            toolStrip1.Items.RemoveByKey("toLbl");
            toolStrip1.Items.RemoveByKey("toMonthTCH");
            toolStrip1.Items.RemoveByKey("nextMonth");
            toolStrip1.Items.RemoveByKey("nextWholeMonth");
        }

        private void ShowPageDataStatusMessage(TabPage page, string message)
        {
            ThreadSafeModify(page, delegate
            {
                Label lbl = page.Controls.Find("lbl_NoData", false)[0] as Label;
                lbl.Text = message;
                lbl.Visible = true;

            });
        }

        private string TabName(TabPage page)
        {
            return page.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
        }

        private string TabDetails(TabPage page)
        {
            return page.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
        }

        private void ToggleToolStripButtons(string module, bool turnon)
        {
            ThreadSafeModify(toolStrip1, delegate { refreshButton.Enabled = false; });
            ThreadSafeModify(toolStrip1, delegate { saveButton.Enabled = false; });
            ThreadSafeModify(toolStrip1, delegate { closeButton.Enabled = false; });
            ThreadSafeModify(toolStrip1, delegate { printButton.Enabled = false; });

            if (turnon)
            {
                if (Modules[module].Actions.Contains(Module.ModuleAction.Refresh))
                    ThreadSafeModify(toolStrip1, delegate { refreshButton.Enabled = true; });
                if (Modules[module].Actions.Contains(Module.ModuleAction.Save))
                    ThreadSafeModify(toolStrip1, delegate { saveButton.Enabled = true; });
                if (Modules[module].Actions.Contains(Module.ModuleAction.Print))
                    ThreadSafeModify(toolStrip1, delegate { printButton.Enabled = true; });
                ThreadSafeModify(toolStrip1, delegate { closeButton.Enabled = true; });
            }
        }

        /* Add security stuff here */
        private void SecureMenu()
        {
            if (!wp.IsInRole("Vantage TimeCard Report"))
                completedLaborOperationsToolStripMenuItem.Visible = false;
            else
                completedLaborOperationsToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Customer Summary"))
                customerSummaryToolStripMenuItem.Visible = false;
            else
                customerSummaryToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Part Usage Report"))
                partUsageToolStripMenuItem.Visible = false;
            else
                partUsageToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Method Tracker"))
                methodTrackerToolStripMenuItem.Visible = false;
            else
                methodTrackerToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Vantage - Zero Cost Parts"))
                zeroCostPartsToolStripMenuItem.Visible = false;
            else
                zeroCostPartsToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Job Material Replace"))
                batchMaterialReplacementToolStripMenuItem.Visible = false;
            else
                batchMaterialReplacementToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Rails Quote Data"))
            {
                railsQuoteDataToolStripMenuItem.Visible = false;
                railsQuoteSummaryToolStripMenuItem.Visible = false;
            }
            else
            {
                railsQuoteDataToolStripMenuItem.Visible = true;
                railsQuoteDataToolStripMenuItem.Visible = true;
            }

            if (!wp.IsInRole("Epicor - PA Records"))
                powerAnalyzerDataToolStripMenuItem.Visible = false;
            else
                powerAnalyzerDataToolStripMenuItem.Visible = true;
            if (!wp.IsInRole("Epicor - Shipment Notifications"))
            {
                shipmentNotificationsToolStripMenuItem.Visible = false;
                sendShipmentNotificationToolStripMenuItem.Visible = false;
            }
            else
            {
                shipmentNotificationsToolStripMenuItem.Visible = true;
                sendShipmentNotificationToolStripMenuItem.Visible = true;
            }

            if (!wp.IsInRole("Epicor - Sales Rep Mgmt"))
                salesRepMgmtToolStripMenuItem.Visible = false;
            else
                salesRepMgmtToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Get Job Manuals"))
                getJobManualsToolStripMenuItem.Visible = false;
            else
                getJobManualsToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Assemblies w_ No Auto Rec"))
                assembliesWNoAutoRecToolStripMenuItem.Visible = false;
            else
                assembliesWNoAutoRecToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Print Part Attach Files"))
            {
                printPartAttachFilesToolStripMenuItem.Visible = false;
                printPartAttachFilesForMOMsToolStripMenuItem.Visible = false;
            }
            else
            {
                printPartAttachFilesToolStripMenuItem.Visible = true;
                printPartAttachFilesForMOMsToolStripMenuItem.Visible = true;
            }

            if (!wp.IsInRole("Epicor - Missing Parts Edit") && !wp.IsInRole("Epicor - Missing Parts View"))
            {
                getPartsBelowReqQtyToolStripMenuItem.Visible = false;
                getOrdersMissingPartsToolStripMenuItem.Visible = false;
                missingPartChecklistToolStripMenuItem.Visible = false;
            }
            else
            {
                getPartsBelowReqQtyToolStripMenuItem.Visible = true;
                getOrdersMissingPartsToolStripMenuItem.Visible = true;
                missingPartChecklistToolStripMenuItem.Visible = true;
            }

            if (!wp.IsInRole("Epicor - Outsourced Material Management"))
                outsourcedMaterialManagementToolStripMenuItem.Visible = false;
            else
                outsourcedMaterialManagementToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Fuzzy Partnum Search"))
                fuzzyPartnumSearchToolStripMenuItem.Visible = false;
            else
                fuzzyPartnumSearchToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Over Ordered Parts"))
                overOrderedPartsToolStripMenuItem.Visible = false;
            else
                overOrderedPartsToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Vantage Part Min Qty Alerts"))
                partMinQtyAlertsToolStripMenuItem.Visible = false;
            else
                partMinQtyAlertsToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Vantage Service Part Min Qty"))
            {
                partMinQtyReportToolStripMenuItem.Visible = false;
                partMinQtyTemplateToolStripMenuItem.Visible = false;
            }
            else
            {
                partMinQtyReportToolStripMenuItem.Visible = true;
                partMinQtyTemplateToolStripMenuItem.Visible = true;
            }

            if (!wp.IsInRole("Vantage Service Part Min Qty"))
                purchaseReceiptSummariesToolStripMenuItem.Visible = false;
            else
                purchaseReceiptSummariesToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Vantage Service Job Dates"))
                editJobOrderDatesToolStripMenuItem.Visible = false;
            else
                editJobOrderDatesToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Vantage Service Part Min Qty"))
                jobMtlIssuedQty25ReqQtyToolStripMenuItem.Visible = false;
            else
                jobMtlIssuedQty25ReqQtyToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Part Time Phase"))
            {
                partTimePhaseWSuppliersToolStripMenuItem.Visible = false;
                partTimePhaseToolStripMenuItem.Visible = false;
            }
            else
            {
                partTimePhaseWSuppliersToolStripMenuItem.Visible = true;
                partTimePhaseToolStripMenuItem.Visible = true;
            }

            newCountGroupToolStripMenuItem.Visible = false;
            customersWNoSOToolStripMenuItem.Visible = false;
            partBinsNotPrimaryToolStripMenuItem.Visible = false;
            contactEmailListToolStripMenuItem.Visible = false;
            /*if (!wp.IsInRole("Epicor - New Count Group"))
                newCountGroupToolStripMenuItem.Visible = false;
            else
                newCountGroupToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Customers No SO"))
                customersWNoSOToolStripMenuItem.Visible = false;
            else
                customersWNoSOToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Part Bins Not Primary"))
                partBinsNotPrimaryToolStripMenuItem.Visible = false;
            else
                partBinsNotPrimaryToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Contact Email List"))
                contactEmailListToolStripMenuItem.Visible = false;
            else
                contactEmailListToolStripMenuItem.Visible = true;*/

            if (!wp.IsInRole("Epicor - Controller"))
                sOBacklogToolStripMenuItem.Visible = false;
            else
                sOBacklogToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Get Operations"))
                jobOperationMetricsToolStripMenuItem.Visible = false;
            else
                jobOperationMetricsToolStripMenuItem.Visible = true;


            sparePartsReportToolStripMenuItem.Visible = false;
            /*if (!wp.IsInRole("Epicor - Spare Parts List"))
                sparePartsReportToolStripMenuItem.Visible = false;
            else
                sparePartsReportToolStripMenuItem.Visible = true;*/

            if (!wp.IsInRole("Epicor - Invoice Reports"))
            {
                invoicesToolStripMenuItem.Visible = false;
                invoicesExtendedToolStripMenuItem.Visible = false;
            }
            else
            {
                //invoicesToolStripMenuItem.Visible = true;
                invoicesExtendedToolStripMenuItem.Visible = true;
            }

            if (!wp.IsInRole("Epicor - Forecast Entry"))
                forecastImportToolStripMenuItem.Visible = false;
            else
                forecastImportToolStripMenuItem.Visible = true;

            if (!wp.IsInRole("Epicor - Customer Info"))
            {
                customerInfoToolStripMenuItem.Visible = false;
                customerExportToolStripMenuItem.Visible = false;
            }
            else
            {
                customerInfoToolStripMenuItem.Visible = true;
                customerExportToolStripMenuItem.Visible = true;
            }

            if (!wp.IsInRole("Epicor - Gross Margin Report"))
            {
                grossMarginToolStripMenuItem.Visible = false;
                grossMarginWRepComToolStripMenuItem.Visible = false;
                grossMarginWFreightToolStripMenuItem.Visible = false;
            }
            else
            {
                //grossMarginToolStripMenuItem.Visible = true;
                grossMarginWRepComToolStripMenuItem.Visible = true;
                //grossMarginWFreightToolStripMenuItem.Visible = true;
            }

            if (!wp.IsInRole("Epicor - Search Sales Orders"))
                salesOrdersToolStripMenuItem.Visible = false;
            else
                salesOrdersToolStripMenuItem.Visible = true;

            if (!partTasksToolStripMenuItem.HasDropDownItems)
                partTasksToolStripMenuItem.Visible = false;
            else
                partTasksToolStripMenuItem.Visible = true;

            if (!jobTasksToolStripMenuItem.HasDropDownItems)
                jobTasksToolStripMenuItem.Visible = false;
            else
                jobTasksToolStripMenuItem.Visible = true;

            if (!customerTasksToolStripMenuItem.HasDropDownItems)
                customerTasksToolStripMenuItem.Visible = false;
            else
                customerTasksToolStripMenuItem.Visible = true;

            if (!purchasingTasksToolStripMenuItem.HasDropDownItems)
                purchasingTasksToolStripMenuItem.Visible = false;
            else
                purchasingTasksToolStripMenuItem.Visible = true;

            if (!reportingTasksToolStripMenuItem.HasDropDownItems)
                reportingTasksToolStripMenuItem.Visible = false;
            else
                reportingTasksToolStripMenuItem.Visible = true;

            if (!tasksToolStripMenuItem.HasDropDownItems)
                tasksToolStripMenuItem.Enabled = false;
            else
                tasksToolStripMenuItem.Enabled = true;

        }

        #endregion

        private void DrawDashboard()
        {
            string taskskey = GetKeyValue("FavoriteTasks");
            string[] tasks = taskskey.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (tasks.Length == 0)
            {
                ThreadSafeModify(label2, delegate { label2.Visible = true; });
                ThreadSafeModify(panel2, delegate { panel2.Visible = false; });
            }
            else
            {
                ThreadSafeModify(label2, delegate { label2.Visible = false; });
                ThreadSafeModify(panel2, delegate { panel2.Visible = true; });
                ThreadSafeModify(flowLayoutPanel1, delegate { flowLayoutPanel1.Controls.Clear(); });
                ThreadSafeModify(flowLayoutPanel2, delegate { flowLayoutPanel2.Controls.Clear(); });
                ThreadSafeModify(flowLayoutPanel3, delegate { flowLayoutPanel3.Controls.Clear(); });
                foreach (string str in tasks)
                {
                    if (Modules.ContainsKey(str))
                    {
                        lock (Modules[str])
                        {
                            LinkLabel lnk = new LinkLabel();
                            lnk.Width = 200;
                            lnk.Text = str;
                            lnk.Click += new EventHandler(ActivateTaskFromControl);
                            ThreadSafeModify(flowLayoutPanel1, delegate { flowLayoutPanel1.Controls.Add(lnk); });

                            Label update = new Label();
                            update.Width = 200;
                            update.Text = "";
                            if (Modules[str].State != Module.ModuleState.Unloaded)
                                update.Text = (Modules[str].LastUpdate.Date == DateTime.Now.Date ? "Today" : Modules[str].LastUpdate.ToShortDateString()) + " " + Modules[str].LastUpdate.ToShortTimeString();
                            ThreadSafeModify(flowLayoutPanel2, delegate { flowLayoutPanel2.Controls.Add(update); });

                            Label count = new Label();
                            count.Width = 200;
                            count.Text = "";
                            switch (Modules[str].State)
                            {
                                case Module.ModuleState.Unloaded:
                                    count.Text = "Not loaded";
                                    break;
                                case Module.ModuleState.Loaded:
                                case Module.ModuleState.Saving:
                                case Module.ModuleState.Printing:
                                    count.Text = Modules[str].Data.Rows.Count.ToString();
                                    break;
                                case Module.ModuleState.Loading:
                                    count.Text = "Loading...";
                                    break;
                            }
                            ThreadSafeModify(flowLayoutPanel3, delegate { flowLayoutPanel3.Controls.Add(count); });
                        }
                    }
                    else
                    {
                        SetKeyValue("FavoriteTasks", taskskey.Replace(str, ""));
                    }
                }
            }
        }

        private void DrawTaskPage(string[] args)
        {
            string task = args[0];

            if (Modules[task].DateFilter)
            {
                List<string> newArgs = new List<string>(args);
                newArgs.Add(Modules[task].From.ToShortDateString());
                newArgs.Add(Modules[task].To.ToShortDateString());
                args = newArgs.ToArray();
            }

            foreach (TabPage page in tabControl1.TabPages)
            {
                if (TabName(page) == task)
                {
                    switch (Modules[task].State)
                    {
                        case Module.ModuleState.Loaded:
                            DataToPage(task, page);
                            break;
                        case Module.ModuleState.Unloaded:
                        case Module.ModuleState.Loading:
                        case Module.ModuleState.Saved:
                            ShowPageDataStatusMessage(page, "Loading data...");
                            if (Modules[task].State == Module.ModuleState.Unloaded && Modules[task].PreLoadDataCaptures.Count > 0)
                            {
                                PopulateDataCaptures(task);
                                dc = new DataCapture();
                                dc.DataCaptures = Modules[task].PreLoadDataCaptures;
                                dc.ShowDialog();
                            }
                            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadData), args);
                            break;
                        case Module.ModuleState.Saving:
                            ShowPageDataStatusMessage(page, "Saving data...");
                            break;
                        case Module.ModuleState.Printing:
                            ShowPageDataStatusMessage(page, "Printing data...");
                            break;
                    }
                }
            }
        }

        private void DataToPage(string module, TabPage page)
        {
            DataGridView dgv = new DataGridView();
            DataGridView dgvt = new DataGridView();

            ThreadSafeModify(page, delegate { dgv = page.Controls.Find("dgv_Data", false)[0] as DataGridView; });

            if (Modules[module].HasTotalRow)
                ThreadSafeModify(page, delegate { dgvt = page.Controls.Find("dgv_Total", false)[0] as DataGridView; });

            ThreadSafeModify(page, delegate { ((Label)page.Controls.Find("lbl_NoData", false)[0]).Visible = false; });

            ThreadSafeModify(dgv, delegate { dgv.Visible = true; });
            ThreadSafeModify(dgvt, delegate { dgvt.Visible = true; });

            lock (Modules[module].Data)
            {
                ThreadSafeModify(dgv, delegate { dgv.DataSource = Modules[module].Data; });
            }

            if (Modules[module].HasTotalRow)
            {
                lock (Modules[module].TotalData)
                {
                    ThreadSafeModify(dgvt, delegate { dgvt.DataSource = Modules[module].TotalData; dgvt.Columns["Total Receipt $"].DefaultCellStyle.Format = "c"; });
                }
            }

            ThreadSafeModify(dgv, delegate { FormatCells(dgv, Modules[module].Columns, module); });
            ThreadSafeModify(dgv, delegate { dgv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells); });
            ThreadSafeModify(dgv, delegate { dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells); });

            bool currPage = false;
            ThreadSafeModify(tabControl1, delegate { currPage = tabControl1.SelectedTab == page; });
            if (currPage)
            {
                ToggleToolStripButtons(module, true);

                if (Modules[module].DateFilter)
                    BuildDateFilter(module);

                foreach (string filter in Modules[module].Filters.Keys)
                {
                    ToolStripLabel lbl = new ToolStripLabel();
                    lbl.Name = "lbl_Filter" + filter.Replace(" ", "");
                    lbl.Text = filter;
                    ThreadSafeModify(toolStrip1, delegate { toolStrip1.Items.Add(lbl); });
                    ToolStripComboBox cbl = new ToolStripComboBox();
                    cbl.Name = "cbl_Filter" + filter.Replace(" ", "");
                    cbl.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbl.SelectedIndexChanged += new EventHandler(filterCB_SelectedIndexChanged);
                    foreach (string key in Modules[module].Filters[filter].Keys)
                        cbl.Items.Add(key);
                    ThreadSafeModify(toolStrip1, delegate { toolStrip1.Items.Add(cbl); });
                    if (Modules[module].SelectedFilter.ContainsKey(filter))
                        ThreadSafeModify(toolStrip1, delegate { ((ToolStripComboBox)toolStrip1.Items[cbl.Name]).SelectedItem = Modules[module].SelectedFilter[filter]; });

                }
            }
        }

        private void ActivateTask(string task)
        {
            bool found = false;

            if (TabName(tabControl1.SelectedTab) == task)
            {
                found = true;
                if (!String.IsNullOrEmpty(Modules[task].Breadcrumb))
                    tabControl1.SelectedTab.Text = task + " -> " + Modules[task].Breadcrumb;
            }
            else
                foreach (TabPage page in tabControl1.TabPages)
                {
                    if (TabName(page) == task)
                    {
                        found = true;
                        if (!String.IsNullOrEmpty(Modules[task].Breadcrumb))
                            page.Text = task + " -> " + Modules[task].Breadcrumb;

                        tabControl1.SelectedTab = page;
                    }
                }

            if (!found)
            {
                string tabname = task;
                if (!String.IsNullOrEmpty(Modules[task].Breadcrumb))
                    tabname = task + " -> " + Modules[task].Breadcrumb;
                TabPage taskPage = new TabPage(tabname);
                taskPage.UseVisualStyleBackColor = true;
                tabControl1.TabPages.Add(taskPage);

                Label lbl = new Label();
                lbl.Name = "lbl_NoData";
                lbl.Font = new System.Drawing.Font("Arial", 14.0f);
                lbl.Padding = new System.Windows.Forms.Padding(20, 20, 0, 0);
                lbl.Dock = DockStyle.Fill;
                lbl.Visible = false;
                taskPage.Controls.Add(lbl);

                DataGridView dgv = new DataGridView();
                dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                dgv.Name = "dgv_Data";
                dgv.Dock = DockStyle.Fill;
                dgv.AllowUserToResizeColumns = true;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.ScrollBars = ScrollBars.Both;
                dgv.ContextMenuStrip = contextMenuStrip1;
                dgv.EditMode = DataGridViewEditMode.EditOnEnter;
                if (Modules[task].AllowDrillDown)
                {
                    DataGridViewLinkColumn c = new DataGridViewLinkColumn();
                    c.UseColumnTextForLinkValue = true;
                    c.Text = "Details";
                    dgv.CellContentClick += new DataGridViewCellEventHandler(dgv_CellContentClick);
                    dgv.Columns.Add(c);
                }
                else if (Modules[task].HasActionColumns())
                {
                    dgv.CellContentClick += new DataGridViewCellEventHandler(dgv_CellContentClick);
                }
                bool updateevent = false;
                foreach (GridColumn column in Modules[task].Columns)
                {
                    if (column.UpdateTriggerColumns.Count > 0 || column.EditTextAction)
                        updateevent = true;
                }
                if (updateevent)
                {
                    dgv.CellEndEdit += new DataGridViewCellEventHandler(dgv_CellEndEdit);
                }
                if (Modules[task].Filters.Count > 0)
                    dgv.Sorted += new EventHandler(dgv_Sorted);
                dgv.Visible = false;
                taskPage.Controls.Add(dgv);

                if (Modules[task].HasTotalRow)
                {
                    DataGridView dgvt = new DataGridView();
                    dgvt.Name = "dgv_Total";
                    dgvt.Height = 50;
                    dgvt.Dock = DockStyle.Bottom;
                    dgvt.AllowUserToResizeColumns = true;
                    dgvt.AllowUserToAddRows = false;
                    dgvt.AllowUserToDeleteRows = false;
                    dgvt.AllowUserToOrderColumns = false;
                    dgvt.Visible = false;
                    dgvt.ScrollBars = ScrollBars.Both;
                    taskPage.Controls.Add(dgvt);
                }
                tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
            }
        }

        /* Add module method call here */
        private void LoadData(object param)
        {
            string[] args = param as string[];
            string module = args[0];

            lock (Modules[module])
            {
                try
                {
                    Modules[module].State = Module.ModuleState.Loading;
                    List<GridColumn> columns = Modules[module].Columns;
                    switch (module)
                    {
                        case "Completed Labor Operations":
                            Modules[module].Data = GetCompletedLaborOperations(columns);
                            break;
                        case "Customer Summary":
                            DoCustomerSummary();
                            break;
                        case "Send Shipment Notification":
                            SendShipmentNotification();
                            break;
                        case "Part Usage":
                            Modules[module].Data = GetPartUsage(columns);
                            break;
                        case "Method Tracker":
                            DoMethodTracker();
                            break;
                        case "Sales Orders":
                            Modules[module].Data = GetLegacySalesOrders(columns);
                            break;
                        case "Order Lines":
                            Modules[module].Data = GetLegacySaleOrderLines(columns, args[1]);
                            break;
                        case "Order Misc Charges":
                            Modules[module].Data = GetLegacySaleOrderMiscCharges(columns, args[1]);
                            break;
                        case "Order Releases":
                            Modules[module].Data = GetLegacySaleOrderReleases(columns, args[1]);
                            break;
                        case "Order Memos":
                            Modules[module].Data = GetLegacySaleOrderMemos(columns, args[1]);
                            break;
                        case "Zero Cost Parts":
                            Modules[module].Data = GetZeroCostParts(columns);
                            break;
                        case "Batch Material Replacement":
                            Modules[module].Data = GetJobBatchMaterialReplacement(columns);
                            break;
                        case "Forecast Import":
                            Modules[module].Data = DoForecastEntry(columns);
                            break;
                        case "Customer Export":
                            Modules[module].Data = GetCustomerExport(columns);
                            break;
                        case "Message Details":
                            Modules[module].Data = GetMessageData(columns, args[1]);
                            break;
                        case "WBM CS/ENG Usage Report":
                            Modules[module].Data = GetCSEngUsageReport(columns);
                            break;
                        case "Rails Quote Data":
                            GetRailsQuoteData(columns);
                            break;
                        case "Rails Quote Summary":
                            Modules[module].Data = GetRailsQuoteSummary(columns);
                            break;
                        case "Power Analyzer Data":
                            Modules[module].Data = GetPowerAnalyzerData(columns);
                            break;
                        case "Shipment Notifications":
                            Modules[module].Data = GetShipmentNotifications(columns);
                            break;
                        case "Sales Rep Mgmt":
                            Modules[module].Data = GetSalesRepMgmt(columns);
                            break;
                        case "Job Operation Metrics":
                            Modules[module].Data = GetJobOperationMetrics(columns);
                            break;
                        case "Gross Margin":
                            Modules[module].Data = GetGrossMarginReport(columns);
                            break;
                        case "Gross Margin w/ Freight":
                            Modules[module].Data = GetGrossMarginFreightReport(columns);
                            break;
                        case "Gross Margin w/ Rep Com":
                            Modules[module].Data = GetGrossMarginRepComReport(columns);
                            break;
                        case "Invoices":
                            Modules[module].Data = GetInvoicesReport(columns);
                            break;
                        case "Invoices Extended":
                            Modules[module].Data = GetInvoicesExtendedReport(columns);
                            break;
                        case "Spare Parts Report":
                            Modules[module].Data = GetSparePartsReport(columns);
                            break;
                        case "Missing Part Checklist":
                            Modules[module].Data = GetMissingPartChecklist(columns);
                            break;
                        case "SO Backlog":
                            Modules[module].Data = GetSOBacklog(columns);
                            break;
                        case "Contact Email List":
                            Modules[module].Data = GetContactEmailList(columns);
                            break;
                        case "Part Bins Not Primary":
                            Modules[module].Data = GetPartBinNotPrimary(columns);
                            break;
                        case "Edit Job/Order Dates":
                            Modules[module].Data = GetJobsMismatchedOrderDates(columns);
                            break;
                        case "Print Part Attach Files For Jobs":
                            Modules[module].Data = GetJobPartsWithAttachments(columns);
                            break;
                        case "Get Jobs Missing Parts":
                            Modules[module].Data = GetJobsMissingParts(columns);
                            break;
                        case "Get Parts Below Req Qty":
                            Modules[module].Data = GetPartsBelowReqQty(columns, args[1]);
                            break;
                        case "Get Parts Below Req Qty Details":
                            Modules[module].Data = GetDetailsForPartBelowReqQty(columns, args[1], args[2], args[3]);
                            break;
                        case "Get Jobs Missing Parts Details":
                            Modules[module].Data = GetJobMissingPartsDetails(columns, args[1]);
                            break;
                        case "Get Sales Orders Missing Parts":
                            Modules[module].Data = GetOrdersMissingParts(columns);
                            break;
                        case "Get Order Parts Below Req Qty":
                            Modules[module].Data = GetOrderPartsBelowReqQty(columns, Int32.Parse(args[1]), Int32.Parse(args[2]), Int32.Parse(args[3]));
                            break;
                        case "Get Order Parts Below Req Qty Details":
                            Modules[module].Data = GetDetailsForPartBelowReqQty(columns, args[1], args[2], args[3]);
                            break;
                        case "Get Job Manuals":
                            Modules[module].Data = GetJobManuals(columns);
                            break;
                        case "Job Manual Page 1":
                            Modules[module].Data = GetJobManualPage1Details(columns, args[1]);
                            break;
                        case "Job Manual Page 2":
                            Modules[module].Data = GetJobManualPage2Details(columns, args[1]);
                            break;
                        case "Assemblies w/ No Auto Rec":
                            Modules[module].Data = GetAssembliesWithNoAutoRec(columns);
                            break;
                        case "Part Min Qty Alerts":
                            Modules[module].Data = GetPartMinQtyAlerts(columns);
                            break;
                        case "Part Min Qty Report":
                            Modules[module].Data = GetPartMinQtyReport(columns);
                            break;
                        case "Part Min Qty Template":
                            Modules[module].Data = GetPartMinQtyTemplate(columns);
                            break;
                        case "Purchase Receipt Summaries":
                            Modules[module].Data = GetPurchaseReceiptSummaries(columns, DateTime.Parse(args[1]), DateTime.Parse(args[2]));
                            Modules[module].TotalData = GetPurchaseReceiptSummariesTotal(Modules[module].Data);
                            break;
                        case "Purchase Receipt Summaries For G/L Account":
                            Modules[module].Data = GetPurchaseReceiptSummariesForGLAccount(columns, args[1], DateTime.Parse(args[2]), DateTime.Parse(args[3]));
                            break;
                        case "Purchase Receipts For Vendor":
                            Modules[module].Data = GetPurchaseReceiptsForGLAccountAndVendor(columns, args[1], args[2], DateTime.Parse(args[3]), DateTime.Parse(args[4]));
                            break;
                        case "Print Part Attach Files For MOMs":
                            Modules[module].Data = GetMOMPartsWithAttachments(columns);
                            break;
                        case "Part Attach File List For Jobs":
                            Modules[module].Data = GetFileAttachListForJob(columns, args[1], args[2]);
                            break;
                        case "Part Attach File List For MOMs":
                            Modules[module].Data = GetFileAttachListForMOM(columns, args[1]);
                            break;
                        case "JobMtl w/ IssuedQty 25% >= ReqQty":
                            Modules[module].Data = GetJobMtlsIssuedQty25PerGTReqQty(columns, DateTime.Parse(args[1]), DateTime.Parse(args[2]));
                            break;
                        case "Outsourced Material Management":
                            Modules[module].Data = GetOutsourcedMaterialManagement(columns);
                            break;
                        case "Fuzzy Partnum Search":
                            Modules[module].Data = GetPartsFuzzyLookup(columns);
                            break;
                        case "Over Ordered Parts":
                            Modules[module].Data = GetPartsOverOrdered(columns);
                            break;
                        case "Part Time Phase":
                            Modules[module].Data = GetPartTimePhase(columns);
                            break;
                        case "Part Time Phase w/ Suppliers":
                            Modules[module].Data = GetPartTimePhaseWithSuppliers(columns);
                            break;
                        case "Customer Info":
                            Modules[module].Data = GetCustomerList(columns);
                            foreach (DataRow row in Modules[module].Data.Rows)
                            {
                                if (!String.IsNullOrEmpty(row["City"].ToString()) && !Modules[module].Filters["City"].ContainsKey(row["City"].ToString()))
                                    Modules[module].Filters["City"].Add(row["City"].ToString(), row["City"].ToString());
                                if (!String.IsNullOrEmpty(row["State"].ToString()) && !Modules[module].Filters["State"].ContainsKey(row["State"].ToString()))
                                    Modules[module].Filters["State"].Add(row["State"].ToString(), row["State"].ToString());
                                if (!String.IsNullOrEmpty(row["Country"].ToString()) && !Modules[module].Filters["Country"].ContainsKey(row["Country"].ToString()))
                                    Modules[module].Filters["Country"].Add(row["Country"].ToString(), row["Country"].ToString());
                            }
                            break;
                        case "New Count Group":
                            Modules[module].Data = NewCountGroup(columns);
                            break;
                        case "Customers w/ No SO":
                            Modules[module].Data = CustomersWNoSO(columns);
                            break;
                    }
                    Modules[module].LastUpdate = DateTime.Now;
                    Modules[module].State = Module.ModuleState.Loaded;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (module != "Method Tracker")
            {
                DrawTaskPage(new string[] { module });
            }
            DrawDashboard();
        }

        /* Add data capture selections here */
        private void PopulateDataCaptures(string task)
        {
            switch (task)
            {
                case "Invoices":
                    Modules["Invoices"].PreLoadDataCaptures["Prod Code"] = GetProdCodes();
                    break;
                case "Customers w/ No SO":
                    Modules["Customers w/ No SO"].PreLoadDataCaptures["Customer Type"] = new Dictionary<string, string>() { { "All", "%" }, { "Customer", "CUS" }, { "Prospect", "PRO" }, { "Suspect", "SUS" } };
                    break;
                case "Part Time Phase":
                    Modules["Part Time Phase"].PreLoadDataCaptures["Plant"] = GetPlants();
                    Modules["Part Time Phase"].PreLoadDataCaptures["Part Class"] = GetPartClasses();
                    break;
                case "Part Time Phase w/ Suppliers":
                    Modules["Part Time Phase w/ Suppliers"].PreLoadDataCaptures["Plant"] = GetPlants();
                    Modules["Part Time Phase w/ Suppliers"].PreLoadDataCaptures["Part Class"] = GetPartClasses();
                    break;
                case "Part Min Qty Alerts":
                    Modules["Part Min Qty Alerts"].PreLoadDataCaptures["Plant"] = GetPlants();
                    Modules["Part Min Qty Alerts"].PreLoadDataCaptures["Part Class"] = GetPartClasses();
                    break;
                case "Part Min Qty Report":
                    Modules["Part Min Qty Report"].PreLoadDataCaptures["Plant"] = GetPlants();
                    Modules["Part Min Qty Report"].PreLoadDataCaptures["Part Class"] = GetPartClasses();
                    break;
                case "Part Min Qty Template":
                    Modules["Part Min Qty Template"].PreLoadDataCaptures["Plant"] = GetPlants();
                    Modules["Part Min Qty Template"].PreLoadDataCaptures["Part Class"] = GetPartClasses();
                    break;
                case "JobMtl w/ IssuedQty 25% >= ReqQty":
                    Modules["JobMtl w/ IssuedQty 25% >= ReqQty"].PreLoadDataCaptures["Plant"] = GetPlants();
                    Modules["JobMtl w/ IssuedQty 25% >= ReqQty"].PreLoadDataCaptures["Part Class"] = GetPartClasses();
                    break;
                case "Print Part Attach Files For Jobs":
                    Modules["Print Part Attach Files For Jobs"].PreLoadDataCaptures["Search Depth"] = new Dictionary<string, string>() { { "Include subassemblies", "1" }, { "Only top level", "0" } };
                    break;
                case "New Count Group":
                    Modules["New Count Group"].PreLoadDataCaptures["Plant"] = GetPlants();
                    Modules["New Count Group"].PreLoadDataCaptures["ABC Code"] = new Dictionary<string, string>() { { "A", "A" }, { "B", "B" }, { "C", "C" } };
                    break;
                case "Get Jobs Missing Parts Details":
                    Modules["Get Jobs Missing Parts Details"].PreLoadDataCaptures["Issue/Return?"] = new Dictionary<string, string>() { { "Yes", "Yes" }, { "No", "No" } };
                    break;
                case "Gross Margin":
                    Modules["Gross Margin"].PreLoadDataCaptures["Customer"] = GetCustomers();
                    Modules[task].PreLoadDataCaptures["Plant"] = GetPlantsForUser();
                    break;
                case "Gross Margin w/ Freight":
                    Modules["Gross Margin w/ Freight"].PreLoadDataCaptures["Customer"] = GetCustomers();
                    Modules[task].PreLoadDataCaptures["Plant"] = GetPlantsForUser();
                    break;
                case "Gross Margin w/ Rep Com":
                    Modules["Gross Margin w/ Rep Com"].PreLoadDataCaptures["Customer"] = GetCustomers();
                    Modules[task].PreLoadDataCaptures["Plant"] = GetPlantsForUser();
                    break;

            }
        }

        /* Comment SecureMenu(); in here for testing */
        private void ShowConnectionWindow()
        {
            ServiceConnection sc = new ServiceConnection();
            string lastUser = GetKeyValue("LastUser");
            if (!String.IsNullOrEmpty(lastUser))
                sc.Username = lastUser;

            if (sc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string connString = "net.tcp://" + VantageServer + "/" + Database;
                try
                {
                    session = new Session(sc.Username, sc.Password, connString, Session.LicenseType.Default, @"\\10.77.146.183\Epicor_Apps\default.sysConfig");
                    SetKeyValue("LastUser", sc.Username);
                    serverStatusLabel.Text = VantageServer + ":" + Database;
                    try
                    {
                        if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                        {
                            serverStatusLabel.Text += " ver" + System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                        }
                        else
                        {
                            serverStatusLabel.Text += " ver UNK";
                        }
                    }
                    catch (Exception ex)
                    {
                        serverStatusLabel.Text += " ver ERR";
                    }
                    plantStatusLabel.Text = session.PlantName;
                    companyStatusLabel.Text = session.CompanyName;
                    userStatusLabel.Text = session.UserID;
                    serverStatusLabel.Visible = true;
                    plantStatusLabel.Visible = true;
                    companyStatusLabel.Visible = true;
                    userStatusLabel.Visible = true;
                    tabControl1.Enabled = true;
                    tasksToolStripMenuItem.Enabled = true;
                    connectToolStripMenuItem.Enabled = false;
                    /* Comment below for testing */
                    SecureMenu();
                    /* */
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshTab(string[] args)
        {
            string tab = args[0];
            foreach (TabPage page in tabControl1.TabPages)
            {
                if (TabName(page) == tab)
                {
                    ToggleToolStripButtons(tab, false);

                    if (Modules[tab].DateFilter)
                        DestroyDateFilter();

                    foreach (string filter in Modules[tab].Filters.Keys)
                    {
                        ThreadSafeModify(toolStrip1, delegate { toolStrip1.Items.RemoveByKey("lbl_Filter" + filter.Replace(" ", "")); });
                        ThreadSafeModify(toolStrip1, delegate { toolStrip1.Items.RemoveByKey("cbl_Filter" + filter.Replace(" ", "")); });
                    }
                    lock (Modules[tab])
                    {
                        if (Modules[tab].State != Module.ModuleState.Saved)
                            Modules[tab].State = Module.ModuleState.Unloaded;
                        Modules[tab].Data = null;
                    }
                    DrawTaskPage(args);
                }
            }
        }

        #endregion

        /* Add module method below */
        #region Modules

        #region Completed Labor Operations

        private DataTable GetCompletedLaborOperations(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                string plant = dc.SelectedValues["Company"] + dc.SelectedValues["Plant"].Substring(1, 2);

                DateTime startdate = new DateTime();
                DateTime enddate = new DateTime();

                if (String.IsNullOrEmpty(dc.SelectedValues["From (MM/DD/YYYY) *blank = yesterday"]))
                    startdate = DateTime.Today.AddDays(-1);
                else if (!DateTime.TryParse(dc.SelectedValues["From (MM/DD/YYYY) *blank = yesterday"], out startdate))
                {
                    MessageBox.Show("Invalid From Date");
                    return dt;
                }

                if (String.IsNullOrEmpty(dc.SelectedValues["To   (MM/DD/YYYY) *blank = yesterday"]))
                    enddate = DateTime.Today.AddDays(-1);
                else if (!DateTime.TryParse(dc.SelectedValues["To   (MM/DD/YYYY) *blank = yesterday"], out enddate))
                {
                    MessageBox.Show("Invalid To Date");
                    return dt;
                }

                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetCompletedLaborOperations @Company, @Plant, @StartDate, @EndDate");
                command.Parameters.AddWithValue("Company", dc.SelectedValues["Company"]);
                command.Parameters.AddWithValue("Plant", plant);
                command.Parameters.AddWithValue("StartDate", startdate);
                command.Parameters.AddWithValue("EndDate", enddate);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion
                
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();

                    newRow["Job #"] = row[0];
                    newRow["Asm #"] = row[1];
                    newRow["Opr"] = row[2];
                    newRow["Part #"] = row[3];
                    newRow["Qty"] = row[4];
                    newRow["Emp #"] = row[5];
                    newRow["Group"] = row[6];

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Part Usage

        private DataTable GetPartUsage(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime trandate = new DateTime();
                if (!String.IsNullOrEmpty(dc.SelectedValues["As Of (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["As Of (MM/DD/YYYY)"], out trandate))
                {
                    MessageBox.Show("As Of is not a valid date");
                }
                else
                {
                    string warehouse = dc.SelectedValues["Plant"];
                    string plant = dc.SelectedValues["Company"] + dc.SelectedValues["Plant"].Substring(1,2);

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_PartUsage @Company, @Warehouse, @Plant, @TranDate");
                    command.Parameters.AddWithValue("Company", dc.SelectedValues["Company"]);
                    command.Parameters.AddWithValue("Warehouse", warehouse);
                    command.Parameters.AddWithValue("Plant", plant);
                    command.Parameters.AddWithValue("TranDate", trandate);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["Part #"] = row[0];
                        newRow["Description"] = row[1];
                        newRow["Class"] = row[2];
                        newRow["Last Usage"] = row[3];
                        newRow["Last Purchase"] = row[4];
                        newRow["1mo Usage"] = row[5];
                        newRow["3mo Usage"] = row[6];
                        newRow["6mo Usage"] = row[7];
                        newRow["12mo Usage"] = row[8];
                        newRow["12mo Usage Cost"] = row[9];
                        newRow["12mo Job Mtl Cost"] = row[10];
                        newRow["24mo Usage"] = row[11];
                        newRow["24mo Usage Cost"] = row[12];
                        newRow["24mo Job Mtl Cost"] = row[13];
                        newRow["On Hand"] = row[14];
                        newRow["Avg Cost"] = row[15];
                        newRow["Pur Last 12mo"] = row[16];
                        newRow["Adj Last 12mo"] = row[17];
                        newRow["36mo Usage"] = row[18];
                        newRow["36mo Usage Cost"] = row[19];
                        newRow["36mo Job Mtl Cost"] = row[20];

                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Customer Summary

        private void DoCustomerSummary()
        {
            try
            {
                string[] companies = new string[] { "CRD", "CIG" };
                int fiscal_year = DateTime.Now.Year;
                if (DateTime.Now.Month < 4)
                    fiscal_year -= 1;

                string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
                ExcelPackage pck = null;
                string filename = dir + " - Customer Summary.xlsx";
                FileInfo newFile = new FileInfo(filename);

                pck = new ExcelPackage(newFile);

                foreach (string company in companies)
                {
                    SqlCommand command = new SqlCommand("exec [dbo].sp_CustomerSummaryExtended @Company");
                    command.Parameters.AddWithValue("Company", company);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);



                        //Add the Content sheet
                        var ws = pck.Workbook.Worksheets.Add(company);


                        ws.Cells[1, 1].Value = "Customer";
                        ws.Cells[1, 2].Value = "City";
                        ws.Cells[1, 3].Value = "State";
                        ws.Cells[1, 4].Value = "Zip";
                        ws.Cells[1, 5].Value = "Rep #";
                        ws.Cells[1, 6].Value = "Mult";
                        ws.Cells[1, 7].Value = "Cust #";
                        ws.Cells[1, 8].Value = "Terr";
                        ws.Cells[1, 9].Value = "Group";
                        ws.Cells[1, 10].Value = "FYTD";
                        ws.Cells[1, 11].Value = "Prior FYTD";
                        ws.Cells[1, 12].Value = "FY"+(fiscal_year-4).ToString();
                        ws.Cells[1, 13].Value = "FY"+(fiscal_year-3).ToString();
                        ws.Cells[1, 14].Value = "FY" + (fiscal_year - 2).ToString();
                    ws.Cells[1, 15].Value = "FY" + (fiscal_year - 1).ToString();
                    ws.Cells[1, 16].Value = "Apr-" + (fiscal_year).ToString();
                    ws.Cells[1, 17].Value = "May-" + (fiscal_year).ToString();
                    ws.Cells[1, 18].Value = "Jun-" + (fiscal_year).ToString();
                    ws.Cells[1, 19].Value = "Jul-" + (fiscal_year).ToString();
                    ws.Cells[1, 20].Value = "Aug-" + (fiscal_year).ToString();
                    ws.Cells[1, 21].Value = "Sep-" + (fiscal_year).ToString();
                    ws.Cells[1, 22].Value = "Oct-" + (fiscal_year).ToString();
                    ws.Cells[1, 23].Value = "Nov-" + (fiscal_year).ToString();
                    ws.Cells[1, 24].Value = "Dec-" + (fiscal_year).ToString();
                    ws.Cells[1, 25].Value = "Jan-" + (fiscal_year+1).ToString();
                    ws.Cells[1, 26].Value = "Feb-" + (fiscal_year+1).ToString();
                    ws.Cells[1, 27].Value = "Mar-" + (fiscal_year+1).ToString();
                    ws.Cells[1, 28].Value = "Apr-" + (fiscal_year-1).ToString();
                    ws.Cells[1, 29].Value = "May-" + (fiscal_year - 1).ToString();
                    ws.Cells[1, 30].Value = "Jun-" + (fiscal_year - 1).ToString();
                    ws.Cells[1, 31].Value = "Jul-" + (fiscal_year - 1).ToString();
                    ws.Cells[1, 32].Value = "Aug-" + (fiscal_year - 1).ToString();
                    ws.Cells[1, 33].Value = "Sep-" + (fiscal_year - 1).ToString();
                    ws.Cells[1, 34].Value = "Oct-" + (fiscal_year - 1).ToString();
                    ws.Cells[1, 35].Value = "Nov-" + (fiscal_year - 1).ToString();
                    ws.Cells[1, 36].Value = "Dec-" + (fiscal_year - 1).ToString();
                    ws.Cells[1, 37].Value = "Jan-" + (fiscal_year).ToString();
                    ws.Cells[1, 38].Value = "Feb-" + (fiscal_year).ToString();
                    ws.Cells[1, 39].Value = "Mar-" + (fiscal_year).ToString();

                    ws.Cells[1, 1, 1, 39].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[1, 1, 1, 39].Style.Font.UnderLine = true;
                        ws.Cells[1, 1, 1, 39].Style.Font.Bold = true;
                        int toprow = 2;
                        bool firstrow = true;

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            ws.Cells[toprow, 1].Value = row[0];
                            ws.Cells[toprow, 2].Value = row[1];
                            ws.Cells[toprow, 3].Value = row[2];
                            ws.Cells[toprow, 4].Value = row[3];
                            ws.Cells[toprow, 5].Value = row[4];
                            ws.Cells[toprow, 6].Value = row[5];
                            ws.Cells[toprow, 7].Value = row[6];
                            ws.Cells[toprow, 8].Value = row[7];
                            ws.Cells[toprow, 9].Value = row[8];
                            ws.Cells[toprow, 10].Value = row[9];
                            ws.Cells[toprow, 11].Value = row[10];
                        ws.Cells[toprow, 12].Value = row[11];
                        ws.Cells[toprow, 13].Value = row[12];
                        ws.Cells[toprow, 14].Value = row[13];
                        ws.Cells[toprow, 15].Value = row[14];
                        ws.Cells[toprow, 16].Value = row[15];
                        ws.Cells[toprow, 17].Value = row[16];
                        ws.Cells[toprow, 18].Value = row[17];
                        ws.Cells[toprow, 19].Value = row[18];
                        ws.Cells[toprow, 20].Value = row[19];
                        ws.Cells[toprow, 21].Value = row[20];
                        ws.Cells[toprow, 22].Value = row[21];
                        ws.Cells[toprow, 23].Value = row[22];
                        ws.Cells[toprow, 24].Value = row[23];
                        ws.Cells[toprow, 25].Value = row[24];
                        ws.Cells[toprow, 26].Value = row[25];
                        ws.Cells[toprow, 27].Value = row[26];
                        ws.Cells[toprow, 28].Value = row[27];
                        ws.Cells[toprow, 29].Value = row[28];
                        ws.Cells[toprow, 30].Value = row[29];
                        ws.Cells[toprow, 31].Value = row[30];
                        ws.Cells[toprow, 32].Value = row[31];
                        ws.Cells[toprow, 33].Value = row[32];
                        ws.Cells[toprow, 34].Value = row[33];
                        ws.Cells[toprow, 35].Value = row[34];
                        ws.Cells[toprow, 36].Value = row[35];
                        ws.Cells[toprow, 37].Value = row[36];
                        ws.Cells[toprow, 38].Value = row[37];
                        ws.Cells[toprow, 39].Value = row[38];

                        toprow++;
                        }

                        ws.Cells[1, 1, toprow, 39].AutoFitColumns();


                    }
                    pck.Save();
                    MessageBox.Show("Results saved to " + dir);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Modules["Customer Summary"].State = Module.ModuleState.Loaded;
            ThreadSafeModify(tabControl1.SelectedTab, delegate {
                string tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                Modules[tab].State = Module.ModuleState.Unloaded;
                refreshButton.Enabled = false;
                saveButton.Enabled = false;
                printButton.Enabled = false;
                closeButton.Enabled = false;
                DestroyDateFilter();

                int index = tabControl1.SelectedIndex;
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                tabControl1.SelectTab(index - 1);
                if (index > 1)
                {
                    tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    ToggleToolStripButtons(tab, true);
                }
            });
        }

        #endregion

        #region Method Tracker

        private void DoMethodTracker()
        {
            decimal burdfact = 1;
            decimal matburdfact = 1;
            if (!Decimal.TryParse(dc.SelectedValues["Burden Factor"], out burdfact))
                MessageBox.Show("Couldn't parse Burden Factor " + dc.SelectedValues["Burden Factor"]);
            else if (!Decimal.TryParse(dc.SelectedValues["Matl Burden Factor"], out matburdfact))
                MessageBox.Show("Couldn't parse Matl Burden Factor " + dc.SelectedValues["Matl Burden Factor"]);
            else
            {

                try
                {
                    SalesOrderImpl soBusObj = WCFServiceSupport.CreateImpl<SalesOrderImpl>(session, SalesOrderImpl.UriPath);
                    
                    foreach (string invoicenumstr in dc.SelectedValues["Invoice #s (comma seperated)"].Split(','))
                    {
                        int invoicenum;
                        if (!Int32.TryParse(invoicenumstr.Trim(), out invoicenum))
                            MessageBox.Show("Couldn't parse Invoice # " + invoicenumstr.Trim());
                        else
                        {
                            SqlCommand command = new SqlCommand("exec [dbo].sp_GetInvoiceOrderLines @Company, @Invoicenum");
                            command.Parameters.AddWithValue("Company", session.CompanyID);
                            command.Parameters.AddWithValue("Invoicenum", invoicenum);
                            DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                            int lastordernum = -1;
                            int lastorderline = -1;
                            int lastorderrelnum = -1;
                            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            ExcelPackage pck = null;
                            foreach (DataRow orderrel in ds.Tables[0].Rows)
                            {
                                int ordernum = Int32.Parse(orderrel[0].ToString());
                                int orderline = Int32.Parse(orderrel[1].ToString());
                                int orderrelnum = Int32.Parse(orderrel[2].ToString());
                                decimal qty = Decimal.Parse(orderrel[3].ToString());

                                if (lastordernum != ordernum)
                                {
                                    lastordernum = ordernum;
                                    string filename = dir + " - SO #" + ordernum.ToString() + ".xlsx";
                                    FileInfo newFile = new FileInfo(filename);

                                    pck = new ExcelPackage(newFile);
                                }
                                SalesOrderDataSet sods = soBusObj.GetByID(ordernum);
                                SalesOrderDataSet.OrderDtlRow line = sods.OrderDtl.FindByCompanyOrderNumOrderLine(session.CompanyID, ordernum, orderline);

                                //Add the Content sheet
                                var ws = pck.Workbook.Worksheets.Add(orderline.ToString() + " - " + line.PartNum);

                                ws.Cells[1, 1].Value = "BOM P/N";
                                ws.Cells[1, 2].Value = line.PartNum;
                                ws.Cells[2, 1].Value = "SO #";
                                ws.Cells[2, 2].Value = ordernum.ToString();
                                ws.Cells[3, 1].Value = "Invoice #";
                                ws.Cells[3, 2].Value = invoicenum.ToString();
                                ws.Cells[4, 1].Value = "Invoice Qty";
                                ws.Cells[4, 2].Value = qty;

                                ws.Cells[6, 2].Value = "BOMType";
                                ws.Cells[6, 3].Value = "BOM Level";
                                ws.Cells[6, 4].Value = "BOM";
                                ws.Cells[6, 5].Value = "Rev";
                                ws.Cells[6, 6].Value = "Part";
                                ws.Cells[6, 7].Value = "Description";
                                ws.Cells[6, 8].Value = "QTY Per";
                                ws.Cells[6, 9].Value = "Req'd";
                                ws.Cells[6, 10].Value = "Matl Unit Cost";
                                ws.Cells[6, 11].Value = "Total Mat'l Cost";
                                ws.Cells[6, 12].Value = "Labor Cost";
                                ws.Cells[6, 13].Value = "Burden Cost";
                                ws.Cells[6, 14].Value = "SubCont.Cost";
                                ws.Cells[6, 15].Value = "Matl Burden Cost";
                                ws.Cells[6, 16].Value = "Total Cost";

                                ws.Cells[6, 2, 6, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[6, 2, 6, 16].Style.Font.UnderLine = true;
                                ws.Cells[6, 2, 6, 16].Style.Font.Bold = true;
                                int toprow = 7;
                                bool firstrow = true;

                                command = new SqlCommand("exec [dbo].sp_BOMwithCost @Company, @PARTNUM, @REV, @Ordernum, @Orderline, @Orderrelnum, @QTY");
                                command.Parameters.AddWithValue("Company", session.CompanyID);
                                command.Parameters.AddWithValue("PARTNUM", line.PartNum);
                                command.Parameters.AddWithValue("REV", line.RevisionNum);
                                command.Parameters.AddWithValue("Ordernum", line.OrderNum);
                                command.Parameters.AddWithValue("Orderline", line.OrderLine);
                                command.Parameters.AddWithValue("Orderrelnum", orderrelnum);
                                command.Parameters.AddWithValue("QTY", qty);
                                DataSet ds2 = SQLAccess.GetDataSet(Server, Database, "", "", command);

                                foreach (DataRow row in ds2.Tables[0].Rows)
                                {
                                    ws.Cells[toprow, 2].Value = row[0].ToString();
                                    ws.Cells[toprow, 3].Value = row[1].ToString();
                                    ws.Cells[toprow, 4].Value = row[2].ToString();
                                    ws.Cells[toprow, 5].Value = row[3].ToString();
                                    ws.Cells[toprow, 6].Value = row[4].ToString();
                                    ws.Cells[toprow, 7].Value = row[5].ToString();
                                    ws.Cells[toprow, 8].Value = row[6].ToString();
                                    ws.Cells[toprow, 9].Value = row[7].ToString();
                                    ws.Cells[toprow, 10].Value = row[8].ToString() == "0.00" ? "-" : row[8].ToString();
                                    ws.Cells[toprow, 11].Value = row[9].ToString() == "0.00" ? "-" : row[9].ToString();
                                    ws.Cells[toprow, 12].Value = row[10].ToString() == "0.00" ? "-" : row[10].ToString();
                                    decimal burden = 0;
                                    ws.Cells[toprow, 13].Value = row[11].ToString() == "0.00" ? "-" : (Decimal.TryParse(row[11].ToString(), out burden) ? Math.Round((burden * burdfact), 2).ToString() : (firstrow ? "ERR" : "-"));
                                    ws.Cells[toprow, 14].Value = row[12].ToString() == "0.00" ? "-" : row[12].ToString();

                                    if (firstrow)
                                    {
                                        decimal matlburden = 0;
                                        ws.Cells[toprow, 15].Value = row[13].ToString() == "0.00" ? "-" : (Decimal.TryParse(row[9].ToString(), out matlburden) ? Math.Round((matlburden * matburdfact), 2).ToString() : "ERR");
                                        decimal labor = 0;
                                        if (!Decimal.TryParse(row[10].ToString(), out labor))
                                            labor = 0;
                                        decimal sub = 0;
                                        if (!Decimal.TryParse(row[12].ToString(), out sub))
                                            sub = 0;

                                        ws.Cells[toprow, 16].Value = Math.Round((Decimal.Parse(row[9].ToString()) + labor + (burden * burdfact) + sub + (matlburden * matburdfact)), 2).ToString();
                                        ws.Cells[toprow, 2, toprow, 16].Style.Font.Bold = true;

                                    }
                                    firstrow = false;
                                    toprow++;
                                }

                                ws.Cells[1, 1, toprow, 16].AutoFitColumns();
                                ws.View.FreezePanes(7, 1);


                            }
                            pck.Save();
                            MessageBox.Show("Results saved to " + dir);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Modules["Method Tracker"].State = Module.ModuleState.Loaded;
                ThreadSafeModify(tabControl1.SelectedTab, delegate { 
                                string tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                                Modules[tab].State = Module.ModuleState.Unloaded;
                                refreshButton.Enabled = false;
                                saveButton.Enabled = false;
                                printButton.Enabled = false;
                                closeButton.Enabled = false;
                                DestroyDateFilter();

                                int index = tabControl1.SelectedIndex;
                                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                                tabControl1.SelectTab(index - 1);
                                if (index > 1)
                                {
                                    tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                                    ToggleToolStripButtons(tab, true);
                                }
                });
            }


        }
        #endregion


        #region LEGACY - Sales Orders

        private System.Data.DataTable GetLegacySalesOrders(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_Legacy_SalesOrder @Company, @OrderNum");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("OrderNum", dc.SelectedValues["Order #"]);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["OpenOrder"] = row[0];
                        newRow["VoidOrder"] = row[1];
                        newRow["Company"] = row[2];
                        newRow["OrderNum"] = row[3];
                        newRow["PONum"] = row[4];
                        newRow["OrderHeld"] = row[5];
                        newRow["EntryPerson"] = row[6];
                        newRow["ShipToNum"] = row[7];
                        newRow["RequestDate"] = row[8];
                        newRow["OrderDate"] = row[9];
                        newRow["FOB"] = row[10];
                        newRow["ShipViaCode"] = row[11];
                        newRow["TermsCode"] = row[12];
                        newRow["DiscountPercent"] = row[13];
                        newRow["PrcConNum"] = row[14];
                        newRow["ShpConNum"] = row[15];
                        newRow["SalesRepList"] = row[16];
                        newRow["OrderComment"] = row[17];
                        newRow["ShipComment"] = row[18];
                        newRow["InvoiceComment"] = row[19];
                        newRow["PickListComment"] = row[20];
                        newRow["DepositBal"] = row[21];
                        newRow["DocDepositBal"] = row[22];
                        newRow["NeedByDate"] = row[23];
                        newRow["CreditOverride"] = row[24];
                        newRow["CreditOverrideUserID"] = row[25];
                        newRow["CreditOverrideLimit"] = row[26];
                        newRow["SndAlrtShp"] = row[27];
                        newRow["ExchangeRate"] = row[28];
                        newRow["CurrencyCode"] = row[29];
                        newRow["LockRate"] = row[30];
                        newRow["CardMemberName"] = row[31];
                        newRow["CardNumber"] = row[32];
                        newRow["CardType"] = row[33];
                        newRow["ExpirationMonth"] = row[34];
                        newRow["ExpirationYear"] = row[35];
                        newRow["CardID"] = row[36];
                        newRow["CardmemberReference"] = row[37];
                        newRow["AllocPriorityCode"] = row[38];
                        newRow["ShipOrderComplete"] = row[39];
                        newRow["EDIOrder"] = row[40];
                        newRow["EDIAck"] = row[41];
                        newRow["Linked"] = row[42];
                        newRow["ICPONum"] = row[43];
                        newRow["ExtCompany"] = row[44];
                        newRow["AckEmailSent"] = row[45];
                        newRow["ApplyOrderBasedDisc"] = row[46];
                        newRow["EntryMethod"] = row[47];
                        newRow["CounterSale"] = row[48];
                        newRow["CreateInvoice"] = row[49];
                        newRow["CreatePackingSlip"] = row[50];
                        newRow["LockQty"] = row[51];
                        newRow["ProcessCard"] = row[52];
                        newRow["CCAmount"] = row[53];
                        newRow["CCFreight"] = row[54];
                        newRow["CCTax"] = row[55];
                        newRow["CCDocAmount"] = row[56];
                        newRow["CCDocFreight"] = row[57];
                        newRow["CCDocTax"] = row[58];
                        newRow["CCStreetAddr"] = row[59];
                        newRow["CCZip"] = row[60];
                        newRow["BTCustNum"] = row[61];
                        newRow["BTConNum"] = row[62];
                        newRow["RepRate4"] = row[63];
                        newRow["RepRate5"] = row[64];
                        newRow["RepSplit1"] = row[65];
                        newRow["RepSplit2"] = row[66];
                        newRow["RepSplit3"] = row[67];
                        newRow["RepSplit4"] = row[68];
                        newRow["RepSplit5"] = row[69];
                        newRow["RepRate1"] = row[70];
                        newRow["RepRate2"] = row[71];
                        newRow["RepRate3"] = row[72];
                        newRow["OutboundSalesDocCtr"] = row[73];
                        newRow["OutboundShipDocsCtr"] = row[74];
                        newRow["DemandContractNum"] = row[75];
                        newRow["DoNotShipBeforeDate"] = row[76];
                        newRow["ResDelivery"] = row[77];
                        newRow["DoNotShipAfterDate"] = row[78];
                        newRow["SatDelivery"] = row[79];
                        newRow["SatPickup"] = row[80];
                        newRow["Hazmat"] = row[81];
                        newRow["DocOnly"] = row[82];
                        newRow["RefNotes"] = row[83];
                        newRow["ApplyChrg"] = row[84];
                        newRow["ChrgAmount"] = row[85];
                        newRow["COD"] = row[86];
                        newRow["CODFreight"] = row[87];
                        newRow["CODCheck"] = row[88];
                        newRow["CODAmount"] = row[89];
                        newRow["GroundType"] = row[90];
                        newRow["NotifyFlag"] = row[91];
                        newRow["NotifyEMail"] = row[92];
                        newRow["DeclaredIns"] = row[93];
                        newRow["DeclaredAmt"] = row[94];
                        newRow["CancelAfterDate"] = row[95];
                        newRow["DemandRejected"] = row[96];
                        newRow["OverrideCarrier"] = row[97];
                        newRow["OverrideService"] = row[98];
                        newRow["CreditCardOrder"] = row[99];
                        newRow["DemandHeadSeq"] = row[100];
                        newRow["PayFlag"] = row[101];
                        newRow["PayAccount"] = row[102];
                        newRow["PayBTAddress1"] = row[103];
                        newRow["PayBTAddress2"] = row[104];
                        newRow["PayBTCity"] = row[105];
                        newRow["PayBTState"] = row[106];
                        newRow["PayBTZip"] = row[107];
                        newRow["PayBTCountry"] = row[108];
                        newRow["DropShip"] = row[109];
                        newRow["CommercialInvoice"] = row[110];
                        newRow["ShipExprtDeclartn"] = row[111];
                        newRow["CertOfOrigin"] = row[112];
                        newRow["LetterOfInstr"] = row[113];
                        newRow["FFID"] = row[114];
                        newRow["FFAddress1"] = row[115];
                        newRow["FFAddress2"] = row[116];
                        newRow["FFCity"] = row[117];
                        newRow["FFState"] = row[118];
                        newRow["FFZip"] = row[119];
                        newRow["FFCountry"] = row[120];
                        newRow["FFContact"] = row[121];
                        newRow["FFCompName"] = row[122];
                        newRow["FFPhoneNum"] = row[123];
                        newRow["IntrntlShip"] = row[124];
                        newRow["AutoPrintReady"] = row[125];
                        newRow["EDIReady"] = row[126];
                        newRow["IndividualPackIDs"] = row[127];
                        newRow["FFAddress3"] = row[128];
                        newRow["DeliveryConf"] = row[129];
                        newRow["AddlHdlgFlag"] = row[130];
                        newRow["NonStdPkg"] = row[131];
                        newRow["ServSignature"] = row[132];
                        newRow["ServAlert"] = row[133];
                        newRow["ServHomeDel"] = row[134];
                        newRow["DeliveryType"] = row[135];
                        newRow["FFCountryNum"] = row[136];
                        newRow["PayBTAddress3"] = row[137];
                        newRow["PayBTCountryNum"] = row[138];
                        newRow["PayBTPhone"] = row[139];
                        newRow["ReadyToCalc"] = row[140];
                        newRow["BTCustNumCustID"] = row[141];
                        newRow["CustomerCustID"] = row[142];
                        newRow["promisedate"] = row[143];
                        newRow["schcode"] = row[144];
                        newRow["frtterms"] = row[145];
                        newRow["bolnotes"] = row[146];
                        newRow["actship"] = row[147];
                        newRow["actshipweight"] = row[148];
                        newRow["thirdpartybill"] = row[149];
                        newRow["markfor"] = row[150];
                        newRow["actshipcharge"] = row[151];

                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetLegacySaleOrderLines(List<GridColumn> columns, string ordernum)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_Legacy_SalesOrder_Line @Company, @OrderNum");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("OrderNum", Int32.Parse(ordernum));
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        if (!String.IsNullOrEmpty(row[127].ToString()))
                        {
                            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(row[127].ToString());
                            using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
                            {
                                using (Stream stream = httpWebReponse.GetResponseStream())
                                {
                                    newRow["Image"] = Image.FromStream(stream);
                                }
                            }
                        }
//                        else
  //                          newRow["Image"] = Properties.Resources.Close_2_icon;

                        newRow["VoidLine"] = row[0];
                        newRow["OpenLine"] = row[1];
                        newRow["Company"] = row[2];
                        newRow["OrderNum"] = row[3];
                        newRow["OrderLine"] = row[4];
                        newRow["LineType"] = row[5];
                        newRow["PartNum"] = row[6];
                        newRow["LineDesc"] = row[7];
                        newRow["Reference"] = row[8];
                        newRow["IUM"] = row[9];
                        newRow["RevisionNum"] = row[10];
                        newRow["POLine"] = row[11];
                        newRow["Commissionable"] = row[12];
                        newRow["DiscountPercent"] = row[13];
                        newRow["UnitPrice"] = row[14];
                        newRow["DocUnitPrice"] = row[15];
                        newRow["OrderQty"] = row[16];
                        newRow["Discount"] = row[17];
                        newRow["DocDiscount"] = row[18];
                        newRow["RequestDate"] = row[19];
                        newRow["ProdCode"] = row[20];
                        newRow["XPartNum"] = row[21];
                        newRow["XRevisionNum"] = row[22];
                        newRow["PricePerCode"] = row[23];
                        newRow["OrderComment"] = row[24];
                        newRow["ShipComment"] = row[25];
                        newRow["InvoiceComment"] = row[26];
                        newRow["PickListComment"] = row[27];
                        newRow["TaxCatID"] = row[28];
                        newRow["AdvanceBillBal"] = row[29];
                        newRow["DocAdvanceBillBal"] = row[30];
                        newRow["QuoteNum"] = row[31];
                        newRow["QuoteLine"] = row[32];
                        newRow["TMBilling"] = row[33];
                        newRow["OrigWhyNoTax"] = row[34];
                        newRow["NeedByDate"] = row[35];
                        newRow["Rework"] = row[36];
                        newRow["RMANum"] = row[37];
                        newRow["RMALine"] = row[38];
                        newRow["ProjectID"] = row[39];
                        newRow["ContractNum"] = row[40];
                        newRow["ContractCode"] = row[41];
                        newRow["Warranty"] = row[42];
                        newRow["WarrantyCode"] = row[43];
                        newRow["MaterialDuration"] = row[44];
                        newRow["LaborDuration"] = row[45];
                        newRow["MiscDuration"] = row[46];
                        newRow["MaterialMod"] = row[47];
                        newRow["LaborMod"] = row[48];
                        newRow["WarrantyComment"] = row[49];
                        newRow["Onsite"] = row[50];
                        newRow["MatCovered"] = row[51];
                        newRow["LabCovered"] = row[52];
                        newRow["MiscCovered"] = row[53];
                        newRow["SalesUM"] = row[54];
                        newRow["SellingFactor"] = row[55];
                        newRow["SellingQuantity"] = row[56];
                        newRow["SalesCatID"] = row[57];
                        newRow["ShipLineComplete"] = row[58];
                        newRow["CumeQty"] = row[59];
                        newRow["CumeDate"] = row[60];
                        newRow["MktgCampaignID"] = row[61];
                        newRow["MktgEvntSeq"] = row[62];
                        newRow["LockQty"] = row[63];
                        newRow["Linked"] = row[64];
                        newRow["ICPONum"] = row[65];
                        newRow["ICPOLine"] = row[66];
                        newRow["ExtCompany"] = row[67];
                        newRow["LastConfigDate"] = row[68];
                        newRow["LastConfigTime"] = row[69];
                        newRow["LastConfigUserID"] = row[70];
                        newRow["ConfigUnitPrice"] = row[71];
                        newRow["ConfigBaseUnitPrice"] = row[72];
                        newRow["PriceListCode"] = row[73];
                        newRow["BreakListCode"] = row[74];
                        newRow["LockPrice"] = row[75];
                        newRow["ListPrice"] = row[76];
                        newRow["DocListPrice"] = row[77];
                        newRow["OrdBasedPrice"] = row[78];
                        newRow["DocOrdBasedPrice"] = row[79];
                        newRow["PriceGroupCode"] = row[80];
                        newRow["OverridePriceList"] = row[81];
                        newRow["PricingValue"] = row[82];
                        newRow["DisplaySeq"] = row[83];
                        newRow["KitParentLine"] = row[84];
                        newRow["KitAllowUpdate"] = row[85];
                        newRow["KitShipComplete"] = row[86];
                        newRow["KitBackFlush"] = row[87];
                        newRow["KitPrintCompsPS"] = row[88];
                        newRow["KitPrintCompsInv"] = row[89];
                        newRow["KitPricing"] = row[90];
                        newRow["KitQtyPer"] = row[91];
                        newRow["SellingFactorDirection"] = row[92];
                        newRow["RepRate1"] = row[93];
                        newRow["RepRate2"] = row[94];
                        newRow["RepRate3"] = row[95];
                        newRow["RepRate4"] = row[96];
                        newRow["RepRate5"] = row[97];
                        newRow["RepSplit1"] = row[98];
                        newRow["RepSplit2"] = row[99];
                        newRow["RepSplit3"] = row[100];
                        newRow["RepSplit4"] = row[101];
                        newRow["RepSplit5"] = row[102];
                        newRow["DemandContractLine"] = row[103];
                        newRow["CreateNewJob"] = row[104];
                        newRow["DoNotShipBeforeDate"] = row[105];
                        newRow["GetDtls"] = row[106];
                        newRow["DoNotShipAfterDate"] = row[107];
                        newRow["SchedJob"] = row[108];
                        newRow["RelJob"] = row[109];
                        newRow["EnableCreateNewJob"] = row[110];
                        newRow["EnableGetDtls"] = row[111];
                        newRow["EnableSchedJob"] = row[112];
                        newRow["EnableRelJob"] = row[113];
                        newRow["CounterSaleWarehouse"] = row[114];
                        newRow["CounterSaleBinNum"] = row[115];
                        newRow["CounterSaleLotNum"] = row[116];
                        newRow["CounterSaleDimCode"] = row[117];
                        newRow["DemandDtlRejected"] = row[118];
                        newRow["KitFlag"] = row[119];
                        newRow["KitsLoaded"] = row[120];
                        newRow["DemandContractNum"] = row[121];
                        newRow["DemandHeadSeq"] = row[122];
                        newRow["DemandDtlSeq"] = row[123];
                        newRow["ReverseCharge"] = row[124];
                        newRow["CustNumCustID"] = row[125];
                        newRow["SAP #"] = row[126];

                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetLegacySaleOrderMiscCharges(List<GridColumn> columns, string ordernum)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_Legacy_SalesOrder_MiscCharge @Company, @OrderNum");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("OrderNum", Int32.Parse(ordernum));
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["Company"] = row[0];
                        newRow["OrderNum"] = row[1];
                        newRow["OrderLine"] = row[2];
                        newRow["SeqNum"] = row[3];
                        newRow["MiscCode"] = row[4];
                        newRow["Description"] = row[5];
                        newRow["MiscAmt"] = row[6];
                        newRow["DocMiscAmt"] = row[7];
                        newRow["FreqCode"] = row[8];
                        newRow["Quoting"] = row[9];
                        newRow["Linked"] = row[10];
                        newRow["ICPONum"] = row[11];
                        newRow["ICPOLine"] = row[12];
                        newRow["ICPOSeqNum"] = row[13];
                        newRow["ExtCompany"] = row[14];

                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private System.Data.DataTable GetLegacySaleOrderReleases(List<GridColumn> columns, string ordernum)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_Legacy_SalesOrder_Release @Company, @OrderNum");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("OrderNum", Int32.Parse(ordernum));
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["Company"] = row[0];
                        newRow["OrderNum"] = row[1];
                        newRow["OrderLine"] = row[2];
                        newRow["OrderRelNum"] = row[3];
                        newRow["LineType"] = row[4];
                        newRow["ReqDate"] = row[5];
                        newRow["OurReqQty"] = row[6];
                        newRow["ShipToNum"] = row[7];
                        newRow["ShipViaCode"] = row[8];
                        newRow["OpenRelease"] = row[9];
                        newRow["FirmRelease"] = row[10];
                        newRow["Make"] = row[11];
                        newRow["OurJobQty"] = row[12];
                        newRow["OurJobShippedQty"] = row[13];
                        newRow["VoidRelease"] = row[14];
                        newRow["OurStockQty"] = row[15];
                        newRow["WarehouseCode"] = row[16];
                        newRow["OurStockShippedQty"] = row[17];
                        newRow["PartNum"] = row[18];
                        newRow["RevisionNum"] = row[19];
                        newRow["TaxExempt"] = row[20];
                        newRow["ShpConNum"] = row[21];
                        newRow["NeedByDate"] = row[22];
                        newRow["Reference"] = row[23];
                        newRow["Plant"] = row[24];
                        newRow["SellingReqQty"] = row[25];
                        newRow["SellingJobQty"] = row[26];
                        newRow["SellingJobShippedQty"] = row[27];
                        newRow["SellingStockQty"] = row[28];
                        newRow["SellingStockShippedQty"] = row[29];
                        newRow["SelectForPicking"] = row[30];
                        newRow["StagingWarehouseCode"] = row[31];
                        newRow["StagingBinNum"] = row[32];
                        newRow["PickError"] = row[33];
                        newRow["CumeQty"] = row[34];
                        newRow["CumeDate"] = row[35];
                        newRow["Linked"] = row[36];
                        newRow["ICPONum"] = row[37];
                        newRow["ICPOLine"] = row[38];
                        newRow["ICPORelNum"] = row[39];
                        newRow["ExtCompany"] = row[40];
                        newRow["ScheduleNumber"] = row[41];
                        newRow["MarkForNum"] = row[42];
                        newRow["DropShipName"] = row[43];
                        newRow["RAN"] = row[44];
                        newRow["DemandReference"] = row[45];
                        newRow["DemandSchedRejected"] = row[46];
                        newRow["DatePickTicketPrinted"] = row[47];
                        newRow["ResDelivery"] = row[48];
                        newRow["SatDelivery"] = row[49];
                        newRow["SatPickup"] = row[50];
                        newRow["VerbalConf"] = row[51];
                        newRow["Hazmat"] = row[52];
                        newRow["DocOnly"] = row[53];
                        newRow["RefNotes"] = row[54];
                        newRow["ApplyChrg"] = row[55];
                        newRow["ChrgAmount"] = row[56];
                        newRow["COD"] = row[57];
                        newRow["CODFreight"] = row[58];
                        newRow["CODCheck"] = row[59];
                        newRow["CODAmount"] = row[60];
                        newRow["GroundType"] = row[61];
                        newRow["NotifyFlag"] = row[62];
                        newRow["NotifyEMail"] = row[63];
                        newRow["DeclaredIns"] = row[64];
                        newRow["DeclaredAmt"] = row[65];
                        newRow["ServSatDelivery"] = row[66];
                        newRow["ServSatPickup"] = row[67];
                        newRow["ServSignature"] = row[68];
                        newRow["ServAlert"] = row[69];
                        newRow["ServPOD"] = row[70];
                        newRow["ServAOD"] = row[71];
                        newRow["ServHomeDel"] = row[72];
                        newRow["DeliveryType"] = row[73];
                        newRow["ServDeliveryDate"] = row[74];
                        newRow["ServPhone"] = row[75];
                        newRow["ServInstruct"] = row[76];
                        newRow["ServRelease"] = row[77];
                        newRow["ServAuthNum"] = row[78];
                        newRow["ServRef1"] = row[79];
                        newRow["ServRef2"] = row[80];
                        newRow["ServRef3"] = row[81];
                        newRow["ServRef4"] = row[82];
                        newRow["ServRef5"] = row[83];
                        newRow["OverrideCarrier"] = row[84];
                        newRow["OverrideService"] = row[85];
                        newRow["DockingStation"] = row[86];
                        newRow["Location"] = row[87];
                        newRow["TransportID"] = row[88];
                        newRow["ShipbyTime"] = row[89];
                        newRow["TaxConnectCalc"] = row[90];
                        newRow["GetDfltTaxIds"] = row[91];

                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private System.Data.DataTable GetLegacySaleOrderMemos(List<GridColumn> columns, string ordernum)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_Legacy_SalesOrder_Memo @Company, @OrderNum");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("OrderNum", Int32.Parse(ordernum));
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["company"] = row[0];
                        newRow["ordernum"] = row[1];
                        newRow["orderline"] = row[2];
                        newRow["orderrelnum"] = row[3];
                        newRow["memodate"] = row[4];
                        newRow["memonum"] = row[5];
                        newRow["memouserid"] = row[6];
                        newRow["notify"] = row[7];
                        newRow["notifyuserid"] = row[8];
                        newRow["notifydate"] = row[9];
                        newRow["memodesc"] = row[10];
                        newRow["memotext"] = row[11];
                        newRow["categoryid"] = row[12];

                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Zero Cost Parts

        private System.Data.DataTable GetZeroCostParts(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetZeroCostParts @Company");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["Part #"] = row[0];
                        newRow["Class"] = row[1];
                        newRow["Description"] = row[2];
                        newRow["On Hand"] = row[3];
                        newRow["Cost"] = row[4];
                        newRow["12mo Usage"] = row[5];
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Job Batch Material Replacement

        private System.Data.DataTable GetJobBatchMaterialReplacement(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                bool mes1=true;
                bool mes2=true;
                if (!String.IsNullOrEmpty(dc.SelectedValues["Original MES"]))
                {
                    SqlCommand command = new SqlCommand("exec [dbo].sp_UD03_MESExists @Company, @MES");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("MES", dc.SelectedValues["Original MES"]);
                    mes1 = Int32.Parse(SQLAccess.GetScalar(Server, Database, "", "", command)) > 0;
                }
                if (!String.IsNullOrEmpty(dc.SelectedValues["New MES (blank for original)"]))
                {
                    SqlCommand command = new SqlCommand("exec [dbo].sp_UD03_MESExists @Company, @MES");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("MES", dc.SelectedValues["New MES (blank for original)"]);
                    mes2 = Int32.Parse(SQLAccess.GetScalar(Server, Database, "", "", command)) > 0;
                }

                PartImpl part = WCFServiceSupport.CreateImpl<PartImpl>(session, PartImpl.UriPath);
                if (!String.IsNullOrEmpty(dc.SelectedValues["New Part # (blank for original)"]) && !part.PartExists(dc.SelectedValues["New Part # (blank for original)"]))
                {
                    MessageBox.Show(dc.SelectedValues["New Part # (blank for original)"] + " does not exist.");
                }
                else if ((!String.IsNullOrEmpty(dc.SelectedValues["Original MES"]) && String.IsNullOrEmpty(dc.SelectedValues["New MES (blank for original)"])) || (String.IsNullOrEmpty(dc.SelectedValues["Original MES"]) && !String.IsNullOrEmpty(dc.SelectedValues["New MES (blank for original)"])))
                {
                    MessageBox.Show("Original MES and New MES must both be popualted");
                }
                else if (!mes1)
                {
                    MessageBox.Show("Original MES " + dc.SelectedValues["Original MES"] + " does not exist");
                }
                else if (!mes2)
                {
                    MessageBox.Show("New MES " + dc.SelectedValues["New MES (blank for original)"] + " does not exist");
                }
                else
                {
                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobMtlUsage @Company, @Partnum");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("Partnum", dc.SelectedValues["Original Part #"]);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["Job #"] = row[0];
                        newRow["Job Part"] = row[1];
                        newRow["Assembly Seq"] = row[2];
                        newRow["Asm Part"] = row[3];
                        newRow["Opr Seq"] = row[4];
                        newRow["Op Code"] = row[5];
                        newRow["Mtl Seq"] = row[6];
                        newRow["Qty"] = row[7];
                        newRow["Part #"] = row[8];
                        newRow["MES"] = dc.SelectedValues["Original MES"];
                        newRow["New Part #"] = dc.SelectedValues["New Part # (blank for original)"];
                        newRow["New MES"] = dc.SelectedValues["New MES (blank for original)"];
                        newRow["New Qty Per"] = dc.SelectedValues["New Qty Per (blank for original)"];
                        newRow["Confirm"] = "Yes";

                        dt.Rows.Add(newRow);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DoJobMaterialReplacement(object state)
        {
            List<DataGridViewRow> rows = state as List<DataGridViewRow>;

            try
            {
                JobEntryImpl je = WCFServiceSupport.CreateImpl<JobEntryImpl>(session, JobEntryImpl.UriPath);

                int id = 0;
                int changed = 0;
                foreach (DataGridViewRow row in rows)
                {
                    JobEntryDataSet ds = je.GetByID(row.Cells["Job #"].Value.ToString());

                    ((JobEntryDataSet.JobHeadRow)ds.JobHead.Rows[0]).JobReleased = false;
                    je.ChangeJobHeadJobReleased(ds);
                    ((JobEntryDataSet.JobHeadRow)ds.JobHead.Rows[0]).JobEngineered = false;
                    je.ChangeJobHeadJobEngineered(ds);
                    je.Update(ds);

                    string effect = "";
                    if (!String.IsNullOrEmpty(row.Cells["New Qty Per"].Value.ToString()))
                    {
                        foreach (JobEntryDataSet.JobMtlRow mtlrow in ds.JobMtl.Rows)
                        {
                            if (mtlrow.AssemblySeq == Int32.Parse(row.Cells["Assembly Seq"].Value.ToString())
                                && mtlrow.RelatedOperation == Int32.Parse(row.Cells["Opr Seq"].Value.ToString())
                                && mtlrow.MtlSeq == Int32.Parse(row.Cells["Mtl Seq"].Value.ToString())
                                && mtlrow.PartNum == row.Cells["Part #"].Value.ToString())
                            {
                                mtlrow.QtyPer = Decimal.Parse(row.Cells["New Qty Per"].Value.ToString());
                                changed += 1;
                                break;
                            }
                        }
                        je.ChangeJobMtlQtyPer(ds);
                        je.Update(ds);
                        effect += "Changed Qty Per to " + row.Cells["New Qty Per"].Value.ToString();
                    }
                    if (!String.IsNullOrEmpty(row.Cells["New Part #"].Value.ToString()))
                    {
                        foreach (JobEntryDataSet.JobMtlRow mtlrow in ds.JobMtl.Rows)
                        {
                            if (mtlrow.AssemblySeq == Int32.Parse(row.Cells["Assembly Seq"].Value.ToString())
                                && mtlrow.RelatedOperation == Int32.Parse(row.Cells["Opr Seq"].Value.ToString())
                                && mtlrow.MtlSeq == Int32.Parse(row.Cells["Mtl Seq"].Value.ToString())
                                && mtlrow.PartNum == row.Cells["Part #"].Value.ToString())
                            {
                                mtlrow.PartNum = row.Cells["New Part #"].Value.ToString();
                                changed += 1;
                                break;
                            }
                        }
                        string newpart = row.Cells["New Part #"].Value.ToString();
                        string msg;
                        bool sub;
                        string type;
                        bool multi;
                        bool complete;
                        string issued;
                        je.ChangeJobMtlPartNum(ds, true, ref newpart, Guid.Empty, "", "", out msg, out sub, out type, out multi, out complete, out issued);
                        je.Update(ds);
                        if (effect.Length > 0)
                            effect += ", ";
                        effect += "Replaced mtl " + row.Cells["Part #"].Value.ToString() + " with " + row.Cells["New Part #"].Value.ToString();
                    }
                    ((JobEntryDataSet.JobHeadRow)ds.JobHead.Rows[0]).JobEngineered = true;
                    je.ChangeJobHeadJobEngineered(ds);

                    ((JobEntryDataSet.JobHeadRow)ds.JobHead.Rows[0]).JobReleased = true;
                    je.ChangeJobHeadJobReleased(ds);

                    ((JobEntryDataSet.JobHeadRow)ds.JobHead.Rows[0]).ChangeDescription = "Automated " + effect;
                    je.Update(ds);

                    if (!String.IsNullOrEmpty(row.Cells["MES"].Value.ToString()))
                    {
                        SqlCommand command = new SqlCommand("exec [dbo].sp_UD03_ReplaceMES @Company, @Jobnum, @MES, @NewMES");
                        command.Parameters.AddWithValue("Company", session.CompanyID);
                        command.Parameters.AddWithValue("Jobnum", row.Cells["Job #"].Value.ToString());
                        command.Parameters.AddWithValue("MES", row.Cells["MES"].Value.ToString());
                        command.Parameters.AddWithValue("NewMES", row.Cells["New MES"].Value.ToString());
                        SQLAccess.NonQuery(Server, Database, "", "", command);
                    }
                }
                MessageBox.Show("Update complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Modules["Batch Material Replacement"].State = Module.ModuleState.Saved;
            RefreshTab(new string[] { "Batch Material Replacement" });
        }

        #endregion

        #region Forecast Entry

        private System.Data.DataTable DoForecastEntry(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                #region Get Data Set


                StreamReader sr = new StreamReader(dc.SelectedValues["Import File"]);
                string text = sr.ReadToEnd();
                sr.Close();
                text = text.Replace("\n", string.Empty);
                string[] lines = text.Split('\r');
                foreach (string line in lines)
                {
                    if (line.Length > 0)
                    {

                        string[] cols = line.Split(',');

                        DataRow newRow = dt.NewRow();
                        newRow["Part"] = cols[0];
                        newRow["Plant"] = cols[1];
                        newRow["Date"] = cols[2];
                        newRow["Qty"] = cols[3];
                        newRow["Customer"] = cols[4];

                        dt.Rows.Add(newRow);

                        int custnum;
                        try
                        {
                            CustomerImpl customer = WCFServiceSupport.CreateImpl<CustomerImpl>(session, CustomerImpl.UriPath);
                            CustomerDataSet cusds = customer.GetByCustID(cols[4].ToString().Trim(), false);
                            custnum = ((CustomerDataSet.CustomerRow)cusds.Customer.Rows[0]).CustNum;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + " - Customer " + cols[4].ToString());
                        }
                        DateTime foredate = DateTime.Parse(cols[2]);
                        decimal foreqty = Decimal.Parse(cols[3]);
                        ForecastImpl forecast = WCFServiceSupport.CreateImpl<ForecastImpl>(session, ForecastImpl.UriPath);
                        bool morePages;
                        ForecastDataSet ds = forecast.GetRows("PartNum='" + cols[0] + "'", "", 0, 0, out morePages);
                        bool found = false;
                        foreach (ForecastDataSet.ForecastRow row in ds.Forecast.Rows)
                        {
                            if (row.CustomerCustID == cols[4] && row.ForeDate == foredate)
                            {
                                found = true;
                                if (foreqty == 0)
                                {
                                        row.Delete();
                                }
                                else
                                {
                                        row.ForeQty = foreqty;
                                }
                            }
                        }
                        if (!found)
                        {
                            if (foreqty > 0)
                            {
                                   forecast.GetNewForecast(ds, cols[0], cols[1], custnum, DateTime.Now);
                                    ((ForecastDataSet.ForecastRow)ds.Forecast.Rows[ds.Forecast.Rows.Count - 1]).ForeDate = foredate;
                                    ((ForecastDataSet.ForecastRow)ds.Forecast.Rows[ds.Forecast.Rows.Count - 1]).ForeQty = foreqty;
                            }
                        }
                        forecast.Update(ds);
                    }
                }

                #endregion

                MessageBox.Show("Forecasts Updated");

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Customer Export

        private System.Data.DataTable GetCustomerExport(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetCustomerExport @Company");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    string filename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + session.CompanyID + "CustomerExport" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    FileInfo newFile = new FileInfo(filename);

                    ExcelPackage pck = new ExcelPackage(newFile);

                    //Add the Content sheet
                    var ws = pck.Workbook.Worksheets.Add("Customer Export");

                    int toprow = 1;

                    //Headers
                    ws.Cells[toprow, 1].Value = "Cust ID";
                    ws.Cells[toprow, 2].Value = "Name";
                    ws.Cells[toprow, 3].Value = "Type";
                    ws.Cells[toprow, 4].Value = "Address 1";
                    ws.Cells[toprow, 5].Value = "Address 2";
                    ws.Cells[toprow, 6].Value = "Address 3";
                    ws.Cells[toprow, 7].Value = "City";
                    ws.Cells[toprow, 8].Value = "State";
                    ws.Cells[toprow, 9].Value = "Zip";
                    ws.Cells[toprow, 10].Value = "Country";
                    ws.Cells[toprow, 11].Value = "Sales Rep";
                    ws.Cells[toprow, 12].Value = "Phone #";
                    ws.Cells[toprow, 13].Value = "Fax #";
                    ws.Cells[toprow, 14].Value = "Email";
                    ws.Cells[toprow, 15].Value = "URL";
                    ws.Cells[toprow, 16].Value = "Established";
                    ws.Cells[toprow, 17].Value = "Discount %";
                    ws.Cells[toprow, 18].Value = "Bill To";
                    ws.Cells[toprow, 19].Value = "Bill To Address 1";
                    ws.Cells[toprow, 20].Value = "Bill To Address 2";
                    ws.Cells[toprow, 21].Value = "Bill To Address 3";
                    ws.Cells[toprow, 22].Value = "Bill To City";
                    ws.Cells[toprow, 23].Value = "Bill To State";
                    ws.Cells[toprow, 24].Value = "Bill To Zip";
                    ws.Cells[toprow, 25].Value = "Bill To Phone";
                    ws.Cells[toprow, 26].Value = "Bill To Fax";

                    ws.Cells[toprow, 1, toprow, 26].Style.Font.Bold = true;

                    toprow++;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ws.Cells[toprow, 1].Value = row[0].ToString();
                        ws.Cells[toprow, 2].Value = row[1].ToString();
                        ws.Cells[toprow, 3].Value = row[2].ToString();
                        ws.Cells[toprow, 4].Value = row[3].ToString();
                        ws.Cells[toprow, 5].Value = row[4].ToString();
                        ws.Cells[toprow, 6].Value = row[5].ToString();
                        ws.Cells[toprow, 7].Value = row[6].ToString();
                        ws.Cells[toprow, 8].Value = row[7].ToString();
                        ws.Cells[toprow, 9].Value = row[8].ToString();
                        ws.Cells[toprow, 10].Value = row[9].ToString();
                        ws.Cells[toprow, 11].Value = row[10].ToString();
                        ws.Cells[toprow, 12].Value = row[11].ToString();
                        ws.Cells[toprow, 13].Value = row[12].ToString();
                        ws.Cells[toprow, 14].Value = row[13].ToString();
                        ws.Cells[toprow, 15].Value = row[14].ToString();
                        ws.Cells[toprow, 16].Value = row[15].ToString();
                        ws.Cells[toprow, 17].Value = row[16].ToString();
                        ws.Cells[toprow, 18].Value = row[17].ToString();
                        ws.Cells[toprow, 19].Value = row[18].ToString();
                        ws.Cells[toprow, 20].Value = row[19].ToString();
                        ws.Cells[toprow, 21].Value = row[20].ToString();
                        ws.Cells[toprow, 22].Value = row[21].ToString();
                        ws.Cells[toprow, 23].Value = row[22].ToString();
                        ws.Cells[toprow, 24].Value = row[23].ToString();
                        ws.Cells[toprow, 25].Value = row[24].ToString();
                        ws.Cells[toprow, 26].Value = row[25].ToString();
                        toprow++;

                        DataRow newRow = dt.NewRow();

                        newRow["Cust ID"] = row[0];
                        newRow["Name"] = row[1];
                        newRow["Type"] = row[2];
                        newRow["Address 1"] = row[3];
                        newRow["Address 2"] = row[4];
                        newRow["Address 3"] = row[5];
                        newRow["City"] = row[6];
                        newRow["State"] = row[7];
                        newRow["Zip"] = row[8];
                        newRow["Country"] = row[9];
                        newRow["Sales Rep"] = row[10];
                        newRow["Phone #"] = row[11];
                        newRow["Fax #"] = row[12];
                        newRow["Email"] = row[13];
                        newRow["URL"] = row[14];
                        newRow["Established"] = row[15];
                        newRow["Discount %"] = row[16];
                        newRow["Bill To"] = row[17];
                        newRow["Bill To Address 1"] = row[18];
                        newRow["Bill To Address 2"] = row[19];
                        newRow["Bill To Address 3"] = row[20];
                        newRow["Bill To City"] = row[21];
                        newRow["Bill To State"] = row[22];
                        newRow["Bill To Zip"] = row[23];
                        newRow["Bill To Phone"] = row[24];
                        newRow["Bill To Fax"] = row[25];

                        dt.Rows.Add(newRow);
                    }

                    ws.Cells[1, 1, toprow, 26].AutoFitColumns();

                    pck.Save();

                    MessageBox.Show("Results saved to " + filename);

                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region WBM CS/Eng Usage Report

        private System.Data.DataTable GetCSEngUsageReport(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime sentdate = new DateTime();
                if (!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (!String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec dbo.sp_GetWBMList @StartDate, @EndDate");
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("StartDate", DBNull.Value);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("EndDate", DBNull.Value);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["Message #"] = row[0];
                        newRow["Message Type"] = row[1];
                        newRow["CS Steps"] = row[2];
                        newRow["CS Time"] = row[3];
                        newRow["Eng Steps"] = row[4];
                        newRow["Eng Time"] = row[5];

                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private System.Data.DataTable GetMessageData(List<GridColumn> columns, string message_id)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec dbo.sp_GetWBMData @MessageID");
                    command.Parameters.AddWithValue("MessageID", message_id);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Dept"] = ds.Tables[0].Rows[i][0];
                        newRow["Step Name"] = ds.Tables[0].Rows[i][2];
                        newRow["User"] = ds.Tables[0].Rows[i][3];
                        if (i == ds.Tables[0].Rows.Count - 1)
                        {
                            newRow["Wait Time"] = "";
                        }
                        else
                        {
                            DateTime endTime = DateTime.Now;
                            if (!String.IsNullOrEmpty(ds.Tables[0].Rows[i + 1][6].ToString()))
                            {
                                endTime = DateTime.Parse(ds.Tables[0].Rows[i + 1][5].ToString());
                            }
                            TimeSpan diff = (endTime - DateTime.Parse(ds.Tables[0].Rows[i][5].ToString()));
                            newRow["Wait Time"] = String.Format("{0:00}:{1:00}:{2:00}", diff.TotalHours, diff.Minutes, diff.Seconds);
                        }
                        dt.Rows.Add(newRow);

                    }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Rails Quote Data

        private void GetRailsQuoteData(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime sentdate = new DateTime();
                if (!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (!String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec dbo.sp_GetRailsQuoteData @StartDate, @EndDate");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("StartDate", DBNull.Value);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("EndDate", DBNull.Value);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    ExcelPackage pck = null;
                    string filename = dir + " - Rails Quote Data.xlsx";
                    FileInfo newFile = new FileInfo(filename);

                    pck = new ExcelPackage(newFile);

                    var ws = pck.Workbook.Worksheets.Add("Data");


                    ws.Cells[1, 1].Value = "Cust #";
                    ws.Cells[1, 2].Value = "Customer";
                    ws.Cells[1, 3].Value = "Job Ref";
                    ws.Cells[1, 4].Value = "Contact";
                    ws.Cells[1, 5].Value = "Sales Rep";
                    ws.Cells[1, 6].Value = "CSR";
                    ws.Cells[1, 7].Value = "Quote Date";
                    ws.Cells[1, 8].Value = "Quote #";
                    ws.Cells[1, 9].Value = "E10 Date";
                    ws.Cells[1, 10].Value = "E10 Quote #";
                    ws.Cells[1, 11].Value = "Convert Days";
                    ws.Cells[1, 12].Value = "Quote Subtotal";
                    ws.Cells[1, 13].Value = "Line #";
                    ws.Cells[1, 14].Value = "Part";
                    ws.Cells[1, 15].Value = "Group";
                    ws.Cells[1, 16].Value = "Description";
                    ws.Cells[1, 17].Value = "Qty";
                    ws.Cells[1, 18].Value = "Price";
                    ws.Cells[1, 19].Value = "Discount %";
                    ws.Cells[1, 20].Value = "Adj Amt";
                    ws.Cells[1, 21].Value = "Adj Code";
                    ws.Cells[1, 22].Value = "Act Ship";
                    ws.Cells[1, 23].Value = "Promise Date";
                    ws.Cells[1, 24].Value = "Ship Date";

                    ws.Cells[1, 1, 1, 24].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[1, 1, 1, 24].Style.Font.UnderLine = true;
                    ws.Cells[1, 1, 1, 24].Style.Font.Bold = true;
                    int toprow = 2;


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        ws.Cells[toprow, 1].Value = row[0];
                        ws.Cells[toprow, 2].Value = row[1];
                        ws.Cells[toprow, 3].Value = row[2];
                        ws.Cells[toprow, 4].Value = row[3];
                        ws.Cells[toprow, 5].Value = row[4];
                        ws.Cells[toprow, 6].Value = row[5];
                        ws.Cells[toprow, 7].Value = row[6];
                        ws.Cells[toprow, 8].Value = row[7];
                        ws.Cells[toprow, 9].Value = row[18];
                        ws.Cells[toprow, 10].Value = row[8];
                        ws.Cells[toprow, 11].Value = row[19];
                        ws.Cells[toprow, 12].Value = row[9];
                        ws.Cells[toprow, 13].Value = row[10];
                        ws.Cells[toprow, 14].Value = row[11];
                        ws.Cells[toprow, 15].Value = row[20];
                        ws.Cells[toprow, 16].Value = row[12];
                        ws.Cells[toprow, 17].Value = row[13];
                        ws.Cells[toprow, 18].Value = row[14];
                        ws.Cells[toprow, 19].Value = row[15];
                        ws.Cells[toprow, 20].Value = row[16];
                        ws.Cells[toprow, 21].Value = row[17];
                        ws.Cells[toprow, 22].Value = row[21];
                        ws.Cells[toprow, 23].Value = row[22];
                        ws.Cells[toprow, 24].Value = row[23];

                        toprow++;
                    }


                    ws.Cells[1, 22, toprow, 24].Style.Numberformat.Format = "mm/dd/yyyy";
                    ws.Cells[1, 1, toprow, 24].AutoFitColumns();

                    pck.Save();
                    MessageBox.Show("Results saved to " + filename);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Modules["Rails Quote Data"].State = Module.ModuleState.Loaded;
            ThreadSafeModify(tabControl1.SelectedTab, delegate {
                string tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                Modules[tab].State = Module.ModuleState.Unloaded;
                refreshButton.Enabled = false;
                saveButton.Enabled = false;
                printButton.Enabled = false;
                closeButton.Enabled = false;
                DestroyDateFilter();

                int index = tabControl1.SelectedIndex;
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                tabControl1.SelectTab(index - 1);
                if (index > 1)
                {
                    tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    ToggleToolStripButtons(tab, true);
                }
            });
        }

        #endregion

        #region Rails Quote Summary

        private System.Data.DataTable GetRailsQuoteSummary(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime sentdate = new DateTime();
                if (!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (!String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand();
                    if (dc.SelectedValues["By"] == "Week")
                        command = new SqlCommand("exec dbo.sp_GetRailsQuoteSummary_Week @StartDate, @EndDate");
                    else if (dc.SelectedValues["By"] == "Product")
                        command = new SqlCommand("exec dbo.sp_GetRailsQuoteSummary_Product @StartDate, @EndDate");

                    if (!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("StartDate", DBNull.Value);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("EndDate", DBNull.Value);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["Group"] = row[0];
                        newRow["Qty"] = row[1];
                        newRow["Quote $"] = row[2];
                        newRow["Order $"] = row[3];
                        newRow["Conversion"] = (Decimal.Parse(row[3].ToString()) / Decimal.Parse(row[2].ToString())).ToString("P");

                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Power Analyzer Data

        private System.Data.DataTable GetPowerAnalyzerData(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime sentdate = new DateTime();
                if (!String.IsNullOrEmpty(dc.SelectedValues["Entry Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["Entry Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("Entry Date is not a valid date");
                }
                else if (!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (!String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else if ((!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) && String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"])) ||
                    (String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) && !String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"])))
                {
                    MessageBox.Show("Start and End Date must be used together");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetPARecordedData @Company, @OrderNum, @EntryDate, @StartDate, @EndDate, @Customer, @Custid");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Order #"]))
                        command.Parameters.AddWithValue("OrderNum", dc.SelectedValues["Order #"]);
                    else
                        command.Parameters.AddWithValue("OrderNum", 0);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Entry Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("EntryDate", dc.SelectedValues["Entry Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("EntryDate", DBNull.Value);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("StartDate", DBNull.Value);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("EndDate", DBNull.Value);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Customer ('like' search)"]))
                        command.Parameters.AddWithValue("Customer", dc.SelectedValues["Customer ('like' search)"]);
                    else
                        command.Parameters.AddWithValue("Customer", DBNull.Value);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Cust ID"]))
                        command.Parameters.AddWithValue("Custid", dc.SelectedValues["Cust ID"]);
                    else
                        command.Parameters.AddWithValue("Custid", DBNull.Value);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Date"] = row[0];
                        newRow["Order #"] = row[1];
                        newRow["Job #"] = row[2];
                        newRow["Assembly #"] = row[3];
                        newRow["Assembly Rec"] = row[4];
                        newRow["Type"] = row[7];
                        newRow["Voltage"] = row[9];
                        newRow["Wattage"] = row[10];
                        newRow["Pf"] = row[11];
                        newRow["Ohms"] = row[12];
                        newRow["Amps"] = row[13];
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SavePowerAnalyzerData(object state)
        {
            try
            {
                if (Modules["Power Analyzer Data"].Data.Rows.Count == 0)
                    MessageBox.Show("Nothing to save");
                else
                {
                    {
                        string filename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PAResults" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        FileInfo newFile = new FileInfo(filename);

                        ExcelPackage pck = new ExcelPackage(newFile);

                        //Add the Content sheet
                        var ws = pck.Workbook.Worksheets.Add("PA Results");

                        int toprow = 1;

                        if (!String.IsNullOrEmpty(dc.SelectedValues["Order #"]))
                        {
                            ws.Cells[toprow, 1].Value = "Order #";
                            ws.Cells[toprow, 2].Value = dc.SelectedValues["Order #"];
                            toprow++;
                        }
                        if (!String.IsNullOrEmpty(dc.SelectedValues["Entry Date (MM/DD/YYYY)"]))
                        {
                            ws.Cells[toprow, 1].Value = "Entry Date";
                            ws.Cells[toprow, 2].Value = dc.SelectedValues["Entry Date (MM/DD/YYYY)"];
                            toprow++;
                        }
                        if (!String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]))
                        {
                            ws.Cells[toprow, 1].Value = "Start Date";
                            ws.Cells[toprow, 2].Value = dc.SelectedValues["Start Date (MM/DD/YYYY)"];
                            toprow++;
                        }
                        if (!String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]))
                        {
                            ws.Cells[toprow, 1].Value = "End Date";
                            ws.Cells[toprow, 2].Value = dc.SelectedValues["End Date (MM/DD/YYYY)"];
                            toprow++;
                        }
                        if (!String.IsNullOrEmpty(dc.SelectedValues["Customer ('like' search)"]))
                        {
                            ws.Cells[toprow, 1].Value = "Customer";
                            ws.Cells[toprow, 2].Value = dc.SelectedValues["Customer ('like' search)"];
                            toprow++;
                        }
                        if (!String.IsNullOrEmpty(dc.SelectedValues["Cust ID"]))
                        {
                            ws.Cells[toprow, 1].Value = "Cust ID";
                            ws.Cells[toprow, 2].Value = dc.SelectedValues["Cust ID"];
                            toprow++;
                        }

                        toprow++;

                        //Headers
                        ws.Cells[toprow, 1].Value = "Date";
                        ws.Cells[toprow, 2].Value = "Order #";
                        ws.Cells[toprow, 3].Value = "Job #";
                        ws.Cells[toprow, 4].Value = "Assembly #";
                        ws.Cells[toprow, 5].Value = "Assembly Rec";
                        ws.Cells[toprow, 6].Value = "Type";
                        ws.Cells[toprow, 7].Value = "Voltage";
                        ws.Cells[toprow, 8].Value = "Wattage";
                        ws.Cells[toprow, 9].Value = "Pf";
                        ws.Cells[toprow, 10].Value = "Ohms";
                        ws.Cells[toprow, 11].Value = "Amps";
                        ws.Cells[toprow, 1, toprow, 11].Style.Font.Bold = true;

                        toprow++;
                        foreach (DataRow row in Modules["Power Analyzer Data"].Data.Rows)
                        {
                            ws.Cells[toprow, 1].Value = row[0].ToString();
                            ws.Cells[toprow, 2].Value = row[1].ToString();
                            ws.Cells[toprow, 3].Value = row[2].ToString();
                            ws.Cells[toprow, 4].Value = row[3].ToString();
                            ws.Cells[toprow, 5].Value = row[4].ToString();
                            ws.Cells[toprow, 6].Value = row[5].ToString();
                            ws.Cells[toprow, 7].Value = row[6].ToString();
                            ws.Cells[toprow, 8].Value = row[7].ToString();
                            ws.Cells[toprow, 9].Value = row[8].ToString();
                            ws.Cells[toprow, 10].Value = row[9].ToString();
                            ws.Cells[toprow, 11].Value = row[10].ToString();
                            toprow++;
                        }

                        ws.Cells[1, 1, toprow, 11].AutoFitColumns();

                        pck.Save();

                        MessageBox.Show("Results saved to " + filename);
                        Modules["Power Analyzer Data"].State = Module.ModuleState.Loaded;
                        DrawTaskPage(new string[] { tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Shipment Notifications

        private void SendShipmentNotification()
        {
            try
            {
                SqlCommand command = new SqlCommand("exec [dbo].sp_ResendShipmentNotification @Packnum");
                command.Parameters.AddWithValue("Packnum", Int32.Parse(dc.SelectedValues["Pack #"]));

                SQLAccess.NonQuery(Server, Database, "", "", command);
                MessageBox.Show("Notification sent");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Modules["Send Shipment Notification"].State = Module.ModuleState.Loaded;
            ThreadSafeModify(tabControl1.SelectedTab, delegate {
                string tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                Modules[tab].State = Module.ModuleState.Unloaded;
                refreshButton.Enabled = false;
                saveButton.Enabled = false;
                printButton.Enabled = false;
                closeButton.Enabled = false;
                DestroyDateFilter();

                int index = tabControl1.SelectedIndex;
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                tabControl1.SelectTab(index - 1);
                if (index > 1)
                {
                    tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    ToggleToolStripButtons(tab, true);
                }
            });


        }

        private void ResendShipmentNotification(string packnum)
        {
            try
            {
                SqlCommand command = new SqlCommand("exec [dbo].sp_ResendShipmentNotification @Packnum");
                command.Parameters.AddWithValue("Packnum", Int32.Parse(packnum));

                SQLAccess.NonQuery(Server, Database, "", "", command);
                MessageBox.Show("Notification sent");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetShipmentNotifications(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime sentdate = new DateTime();
                if (!String.IsNullOrEmpty(dc.SelectedValues["Sent Date (MM/DD/YYYY)"]) && !DateTime.TryParse(dc.SelectedValues["Sent Date (MM/DD/YYYY)"], out sentdate))
                {
                    MessageBox.Show("Sent Date is not a valid date");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetShipmentNotifications @CustID, @OrderNum, @PackNum, @SentDate");
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Cust ID"]))
                        command.Parameters.AddWithValue("CustID", dc.SelectedValues["Cust ID"]);
                    else
                        command.Parameters.AddWithValue("CustID", '%');
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Order #"]))
                        command.Parameters.AddWithValue("OrderNum", dc.SelectedValues["Order #"]);
                    else
                        command.Parameters.AddWithValue("OrderNum", 0);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Pack #"]))
                        command.Parameters.AddWithValue("PackNum", dc.SelectedValues["Pack #"]);
                    else
                        command.Parameters.AddWithValue("PackNum", 0);
                    if (!String.IsNullOrEmpty(dc.SelectedValues["Sent Date (MM/DD/YYYY)"]))
                        command.Parameters.AddWithValue("SentDate", dc.SelectedValues["Sent Date (MM/DD/YYYY)"]);
                    else
                        command.Parameters.AddWithValue("SentDate", '%');
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Cust ID"] = row[0];
                        newRow["Pro #"] = row[5];
                        newRow["Order #"] = row[1];
                        newRow["Pack #"] = row[2];
                        newRow["Sent"] = row[3];
                        newRow["Recipient"] = row[4];
                        newRow["Resend"] = "resend";
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Job Operation Metrics

        private System.Data.DataTable GetJobOperationMetrics(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime outstartdate = new DateTime();
                DateTime outenddate = new DateTime();
                if (String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out outstartdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out outenddate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobOperationStats @Company, @StartDate, @EndDate");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);

                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Job #"] = row[0];
                        newRow["Date"] = row[1];
                        newRow["Part #"] = row[2];
                        newRow["Opr Seq"] = row[3];
                        newRow["Opr Code"] = row[4];
                        newRow["Qty"] = row[5];
                        newRow["Hours"] = row[6];
                        newRow["Avg"] = row[7];
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Gross Margin Report

        public Dictionary<string, string> GetCustomers()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetCustomersForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", session.CompanyID);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", sqlCommand);

                result.Add("All", "%");
                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[0].ToString(), row[1].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        private System.Data.DataTable GetGrossMarginReport(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime outstartdate = new DateTime();
                DateTime outenddate = new DateTime();
                Decimal burvar = new Decimal();
                Decimal matvar = new Decimal();
                if (String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out outstartdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out outenddate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["Burden Var %"]) || !Decimal.TryParse(dc.SelectedValues["Burden Var %"], out burvar))
                {
                    MessageBox.Show("Burden Var % is not a valid value");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["Material Var %"]) || !Decimal.TryParse(dc.SelectedValues["Material Var %"], out matvar))
                {
                    MessageBox.Show("Material Var % is not a valid value");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_CISREPORT_GrossMargin @Company, @Plant, @StartDate, @EndDate, @BurdenVar, @MaterialVar, @Customer");
                    string company = dc.SelectedValues["Plant"].Substring(0, 3);
                    command.Parameters.AddWithValue("Company", company);
                    command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                    command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("BurdenVar", burvar / (decimal)100);
                    command.Parameters.AddWithValue("MaterialVar", matvar / (decimal)100);
                    command.Parameters.AddWithValue("Customer", dc.SelectedValues["Customer"]);

                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Invoice #"] = row[0];
                        newRow["Line #"] = row[1];
                        newRow["Plant"] = row[2];
                        newRow["Invoice Date"] = row[3];
                        newRow["Order #"] = row[4];
                        newRow["Cust ID"] = row[5];
                        newRow["Customer"] = row[6];
                        newRow["PO #"] = row[7];
                        newRow["Part #"] = row[8];
                        newRow["Line Desc"] = row[9];
                        newRow["Doc Discount"] = row[10];
                        newRow["Unit Price"] = row[11];
                        newRow["Total Price"] = row[12];
                        newRow["Invoice - Our Ship Qty"] = row[13];
                        newRow["Labor Cost"] = row[14];
                        newRow["Burden Cost"] = row[15];
                        newRow["Burden Var"] = row[16];
                        newRow["Material Cost"] = row[17];
                        newRow["Material Var"] = row[18];
                        newRow["Subunit Cost"] = row[19];
                        newRow["Total Cost"] = row[20];
                        newRow["Margin"] = row[21];
                        newRow["Product Code"] = row[22];
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Gross Margin Report w/ Freight

        private System.Data.DataTable GetGrossMarginFreightReport(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime outstartdate = new DateTime();
                DateTime outenddate = new DateTime();
                Decimal burvar = new Decimal();
                Decimal matvar = new Decimal();
                if (String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out outstartdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out outenddate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["Burden Var %"]) || !Decimal.TryParse(dc.SelectedValues["Burden Var %"], out burvar))
                {
                    MessageBox.Show("Burden Var % is not a valid value");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["Material Var %"]) || !Decimal.TryParse(dc.SelectedValues["Material Var %"], out matvar))
                {
                    MessageBox.Show("Material Var % is not a valid value");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_CISREPORT_GrossMarginWFreight @Company, @Plant, @StartDate, @EndDate, @BurdenVar, @MaterialVar, @Customer");
                    string company = dc.SelectedValues["Plant"].Substring(0, 3);
                    command.Parameters.AddWithValue("Company", company);
                    command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                    command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("BurdenVar", burvar / (decimal)100);
                    command.Parameters.AddWithValue("MaterialVar", matvar / (decimal)100);
                    command.Parameters.AddWithValue("Customer", dc.SelectedValues["Customer"]);

                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Invoice #"] = row[0];
                        newRow["Line #"] = row[1];
                        newRow["Plant"] = row[2];
                        newRow["Invoice Date"] = row[3];
                        newRow["Order #"] = row[4];
                        newRow["Cust ID"] = row[5];
                        newRow["Customer"] = row[6];
                        newRow["PO #"] = row[7];
                        newRow["Part #"] = row[8];
                        newRow["Line Desc"] = row[9];
                        newRow["Doc Discount"] = row[10];
                        newRow["Unit Price"] = row[11];
                        newRow["Total Price"] = row[12];
                        newRow["Invoice - Our Ship Qty"] = row[13];
                        newRow["Labor Cost"] = row[14];
                        newRow["Burden Cost"] = row[15];
                        newRow["Burden Var"] = row[16];
                        newRow["Material Cost"] = row[17];
                        newRow["Material Var"] = row[18];
                        newRow["Subunit Cost"] = row[19];
                        newRow["Total Cost"] = row[20];
                        newRow["Margin"] = row[21];
                        newRow["Product Code"] = row[22];
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Gross Margin w/ Rep Com Report

        private System.Data.DataTable GetGrossMarginRepComReport(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime outstartdate = new DateTime();
                DateTime outenddate = new DateTime();
                Decimal burvar = new Decimal();
                Decimal matvar = new Decimal();
                if (String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out outstartdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out outenddate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["Burden Var %"]) || !Decimal.TryParse(dc.SelectedValues["Burden Var %"], out burvar))
                {
                    MessageBox.Show("Burden Var % is not a valid value");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["Material Var %"]) || !Decimal.TryParse(dc.SelectedValues["Material Var %"], out matvar))
                {
                    MessageBox.Show("Material Var % is not a valid value");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_CISREPORT_GrossMarginWCom @Company, @Plant, @StartDate, @EndDate, @BurdenVar, @MaterialVar, @Customer");
                    string company = dc.SelectedValues["Plant"].Substring(0, 3);
                    command.Parameters.AddWithValue("Company", company);
                    command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                    command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("BurdenVar", burvar / (decimal)100);
                    command.Parameters.AddWithValue("MaterialVar", matvar / (decimal)100);
                    command.Parameters.AddWithValue("Customer", dc.SelectedValues["Customer"]);

                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Invoice #"] = row[0];
                        newRow["Plant"] = row[1];
                        newRow["Invoice Date"] = row[2];
                        newRow["Order #"] = row[3];
                        newRow["Cust ID"] = row[4];
                        newRow["Customer"] = row[5];
                        newRow["Sales Rep"] = row[6];
                        newRow["PO #"] = row[7];
                        newRow["Part #"] = row[8];
                        newRow["Line Desc"] = row[9];
                        newRow["Doc Discount"] = row[10];
                        newRow["Unit Price"] = row[11];
                        newRow["Total Price"] = row[12];
                        newRow["Invoice - Our Ship Qty"] = row[13];
                        newRow["Labor Cost"] = row[14];
                        newRow["Burden Cost"] = row[15];
                        newRow["Burden Var"] = row[16];
                        newRow["Material Cost"] = row[17];
                        newRow["Material Var"] = row[18];
                        newRow["Subunit Cost"] = row[19];
                        newRow["Noncomissionable"] = row[20];
                        newRow["Comissionable"] = row[21];
                        newRow["Rate"] = row[22];
                        newRow["Comission"] = row[23];
                        newRow["Total Cost"] = row[24];
                        newRow["Margin"] = row[25];
                        newRow["Product Code"] = row[26];
                        newRow["Order Line"] = row[27];
                        newRow["System"] = row[28];
                        newRow["Size"] = row[29];
                        newRow["Finish"] = row[30];
                        newRow["Light"] = row[31];
                        newRow["Lights"] = row[32];
                        newRow["Locks"] = row[33];
                        newRow["Handle"] = row[34];
                        newRow["Openings"] = row[35];
                        newRow["Width"] = row[36];
                        newRow["Height"] = row[37];
                        newRow["Area"] = row[38];

                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Invoices Report

        public Dictionary<string, string> GetProdCodes()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetProdCodesForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", session.CompanyID);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", sqlCommand);

                result.Add("All", "%");
                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[1].ToString().Trim(), row[0].ToString().Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        private System.Data.DataTable GetInvoicesReport(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime outstartdate = new DateTime();
                DateTime outenddate = new DateTime();
                if (String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out outstartdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out outenddate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetInvoiceReport @Company, @StartDate, @EndDate");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("ProdCode", dc.SelectedValues["Prod Code"]);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                    newRow["Address 1"] = row[0];
                    newRow["Address 2"] = row[1];
                    newRow["City"] = row[2];
                    newRow["State"] = row[3];
                    newRow["Zip"] = row[4];
                    newRow["Invoice #"] = row[5];
                    newRow["Invoice Date"] = row[6];
                    newRow["Order #"] = row[7];
                    newRow["Cust ID"] = row[8];
                    newRow["Customer"] = row[9];
                    newRow["PO #"] = row[10];
                    newRow["Part #"] = row[11];
                    newRow["Line Desc"] = row[12];
                    newRow["Doc Discount"] = row[13];
                    newRow["Unit Price"] = row[14];
                    newRow["Misc Code"] = row[15];
                    newRow["Misc Amt"] = row[16];
                    newRow["Total Price"] = row[17];
                    newRow["Shipment - Our Ship Qty"] = row[18];
                    newRow["Invoice - Our Ship Qty"] = row[19];
                    newRow["Labor Cost"] = row[20];
                    newRow["Burden Cost"] = row[21];
                    newRow["Material Cost"] = row[22];
                    newRow["Subunit Cost"] = row[23];
                    newRow["Total Cost"] = row[24];
                    newRow["Margin"] = row[25];
                    newRow["Product Code"] = row[26];
                    newRow["Light Type"] = row[27];
                    newRow["Order Date"] = row[28];
                    newRow["Plant"] = row[29];
                    newRow["Sch Code"] = row[30];
                    newRow["Mark For"] = row[31];
                    newRow["Promise Date"] = row[32];
                    newRow["Need By Date"] = row[33];
                    newRow["Pack #"] = row[34];
                    newRow["Tracking #"] = row[35];
                    newRow["Ship Via"] = row[36];
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Invoices Extended Report

        private System.Data.DataTable GetInvoicesExtendedReport(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime outstartdate = new DateTime();
                DateTime outenddate = new DateTime();
                if (String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out outstartdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out outenddate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetInvoiceExtendedReport @Company, @StartDate, @EndDate");
                    if (dc.SelectedValues["Part Class"] == "ModLine")
                    {
                        command = new SqlCommand("exec [dbo].sp_GetInvoiceExtendedForModlineReport @Company, @StartDate, @EndDate");
                    }
                    else if (dc.SelectedValues["Part Class"] != "%" && session.CompanyID == "CRD")
                    {
                        command = new SqlCommand("exec [dbo].sp_GetInvoiceExtendedForSystemReport @Company, @StartDate, @EndDate, @Line");
                        command.Parameters.AddWithValue("Line", dc.SelectedValues["Part Class"]);
                    }
                    else
                    {
                        command = new SqlCommand("exec [dbo].sp_GetInvoiceExtendedReport @Company, @StartDate, @EndDate, @PartClass");
                        command.Parameters.AddWithValue("PartClass", dc.SelectedValues["Part Class"]);
                    }
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Address 1"] = row[0];
                        newRow["Address 2"] = row[1];
                        newRow["City"] = row[2];
                        newRow["State"] = row[3];
                        newRow["Zip"] = row[4];
                        newRow["Invoice #"] = row[5];
                        newRow["Invoice Date"] = row[6];
                        newRow["Order #"] = row[7];
                        newRow["Cust ID"] = row[8];
                        newRow["Customer"] = row[9];
                        newRow["PO #"] = row[10];
                        newRow["Part #"] = row[11];
                        newRow["Line Desc"] = row[12];
                        newRow["Doc Discount"] = row[13];
                        newRow["Unit Price"] = row[14];
                        newRow["Misc Code"] = row[15];
                        newRow["Misc Amt"] = row[16];
                        newRow["Total Price"] = row[17];
                        newRow["Shipment - Our Ship Qty"] = row[18];
                        newRow["Invoice - Our Ship Qty"] = row[19];
                        newRow["Labor Cost"] = row[20];
                        newRow["Burden Cost"] = row[21];
                        newRow["Material Cost"] = row[22];
                        newRow["Subunit Cost"] = row[23];
                        newRow["Total Cost"] = row[24];
                        newRow["Margin"] = row[25];
                        newRow["Product Code"] = row[26];
                    newRow["Construction Name"] = row[27];
                    newRow["Construction Code"] = row[28];
                    newRow["Num Doors"] = row[29];
                    newRow["Door Size"] = row[30];
                    newRow["Light Type"] = row[31];
                    newRow["LED Type"] = row[32];
                    newRow["Handle"] = row[33];
                    newRow["Finish"] = row[34];
                    newRow["Locks"] = row[35];
                    newRow["Full Silkscreen"] = row[36];
                    newRow["Back Kickplate"] = row[37];
                    newRow["Front Kickplate"] = row[38];
                    newRow["Back Bumperguard"] = row[39];
                    newRow["Front Bumperguard"] = row[40];
                    newRow["Inlay"] = row[41];
                    newRow["Order Date"] = row[42];
                    newRow["Plant"] = row[43];
                    newRow["Sch Code"] = row[44];
                    newRow["Mark For"] = row[45];
                    dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Spare Parts Report

        private System.Data.DataTable GetSparePartsReport(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime outstartdate = new DateTime();
                DateTime outenddate = new DateTime();
                if (String.IsNullOrEmpty(dc.SelectedValues["Start Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["Start Date (MM/DD/YYYY)"], out outstartdate))
                {
                    MessageBox.Show("Start Date is not a valid date");
                }
                else if (String.IsNullOrEmpty(dc.SelectedValues["End Date (MM/DD/YYYY)"]) || !DateTime.TryParse(dc.SelectedValues["End Date (MM/DD/YYYY)"], out outenddate))
                {
                    MessageBox.Show("End Date is not a valid date");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetSparePartsReport @Company, @StartDate, @EndDate, @PartsList");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("StartDate", dc.SelectedValues["Start Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("EndDate", dc.SelectedValues["End Date (MM/DD/YYYY)"]);
                    command.Parameters.AddWithValue("PartsList", dc.SelectedValues["Part List (comma seperated)"]);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Part #"] = row[0];
                        newRow["Description"] = row[1];
                        newRow["$ Sold"] = row[2];
                        newRow["Qty Sold"] = row[3];
                        newRow["Latest Part Cost"] = row[4];
                        newRow["Last Purchase/Fabricate Date"] = row[5];
                        newRow["Extended Cost"] = row[6];
                        newRow["Net Margin $"] = row[7];
                        newRow["Net Margin %"] = row[8];
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Missing Part Checklist

        private System.Data.DataTable GetMissingPartChecklist(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobMtls @Company, @Jobnum");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Jobnum", dc.SelectedValues["Job #"]);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if ((dc.SelectedValues["Missing Only"] == "Yes" && Decimal.Parse(row[3].ToString()) < Decimal.Parse(row[2].ToString())) || dc.SelectedValues["Missing Only"] == "No")
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Job #"] = row[0];
                        newRow["Mtl Part #"] = row[1];
                        newRow["Req Qty"] = row[2];
                        newRow["Issued Qty"] = row[3];
                        newRow["Assembly"] = row[4];
                        newRow["Operation"] = row[5];
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrintMissingPartChecklist(object state)
        {
            object[] args = state as object[];
            try
            {
                string ReportTitle = args[0] as string;
                DataGridViewSelectedRowCollection selectedRows = args[1] as DataGridViewSelectedRowCollection;
                StringBuilder builder = new StringBuilder();
                builder.Append("<html><title>" + ReportTitle + "</title><body>");
                builder.Append("<h3>" + ReportTitle + "</h3>");
                builder.Append("<h5>All Raw Material or parts not Issued to the Job</h5>");
                builder.Append("<h5>Job #" + selectedRows[0].Cells[0].Value + "</h5>");
                StringBuilder data = new StringBuilder();
                string lastasm = "";
                string lastop = "";
                builder.Append("<table><tbody>");
                for (int i = selectedRows.Count - 1; i >= 0; i--)
                {
                    DataGridViewRow row = selectedRows[i];
                    if ((dc.SelectedValues["Missing Only"] == "Yes" && Decimal.Parse(row.Cells[3].Value.ToString()) < Decimal.Parse(row.Cells[2].Value.ToString())) || dc.SelectedValues["Missing Only"] == "No")
                    {
                        if (String.IsNullOrEmpty(lastasm) || row.Cells[4].Value.ToString() != lastasm)
                        {
                            builder.Append("<tr><td colspan=\"3\"><h5>" + row.Cells[4].Value + "</h5></td></tr>");
                            lastasm = row.Cells[4].Value.ToString();
                            lastop = "";
                        }

                        if (String.IsNullOrEmpty(lastop) || row.Cells[5].Value.ToString() != lastop)
                        {
                            builder.Append("<tr><td colspan=\"3\"><h5>" + row.Cells[5].Value + "</h5></td></tr>");
                            lastop = row.Cells[5].Value.ToString();
                        }

                        string imagetag = "";
                        if (Decimal.Parse(row.Cells[3].Value.ToString()) >= Decimal.Parse(row.Cells[2].Value.ToString()))
                        {
                            imagetag = "<img src=\"file://" + Path.GetFullPath(".") + "/Resources/check_mark_green.png\" width=\"20px\" height=\"20px\" />";
                        }
                        else
                        {
                            imagetag = "<img src=\"file://" + Path.GetFullPath(".") + "/Resources/red_x.png\" width=\"20px\" height=\"20px\" />";
                        }

                        builder.Append("<tr>" +
                            "<td>" + imagetag + "</td>" +
                            "<td style=\"margin-right: 50px;\">" + row.Cells[1].Value + "</td>" +
                            "<td>" + row.Cells[3].Value.ToString() + " of " + row.Cells[2].Value.ToString() + "</td>" +
                            "</tr>");
                    }
                }

                builder.Append(data.ToString());
                builder.Append("</tbody></table>");
                builder.Append("</body></html>");

                webBrowser1.DocumentText = builder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SO Backlog

        private System.Data.DataTable GetSOBacklog(List<GridColumn> columns)
        {
            try
            {
                System.Data.DataTable dt = GenerateGridTable(columns);

                DateTime outdate = new DateTime();
                if (String.IsNullOrEmpty(dc.SelectedValues["As Of Date (mm/dd/yyyy)"]) || !DateTime.TryParse(dc.SelectedValues["As Of Date (mm/dd/yyyy)"], out outdate))
                {
                    MessageBox.Show("As Of Date is not a valid date");
                }
                else
                {

                    #region Get Data Set

                    SqlCommand command = new SqlCommand("exec [dbo].sp_SOAsOfBacklog @Company, @AsOfDate");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("AsOfDate", outdate);
                    DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    #endregion

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["Order Date"] = row[0];
                        newRow["Order #"] = row[1];
                        newRow["Order Line #"] = row[2];
                        newRow["Order Rel #"] = row[3];
                        newRow["Part #"] = row[4];
                        newRow["Quantity"] = row[5];
                        newRow["Unit Price"] = row[6];
                        newRow["Ext Price"] = row[7];
                        newRow["Ship Date"] = row[8];
                        dt.Rows.Add(newRow);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Contact Email List

        private System.Data.DataTable GetContactEmailList(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetUnprocessedContactEmails @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Contact Name"] = row[1];
                    newRow["Email Address"] = row[0];
                    dt.Rows.Add(newRow);
                    command = new SqlCommand("exec [dbo].sp_ProcessContactEmail @Company, @Email");
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("Email", row[0].ToString());
                    SQLAccess.NonQuery(Server, Database, "", "", command);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Part Bin Not Primary

        private System.Data.DataTable GetPartBinNotPrimary(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetPartBinsNotPrimary @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part #"] = row[0];
                    newRow["Plant"] = row[1];
                    newRow["Warehouse"] = row[2];
                    newRow["Primary Bin"] = row[3];
                    newRow["Secondary Bin"] = row[4];
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Customers w/ No SO

        /*private void BuildCustomerQuery(DynamicQuery dqBusObj, string jobName, string customertype)
        {
            QueryDesignDataSet qds = new QueryDesignDataSet();
            dqBusObj.GetNewQuery(qds);
            qds.DynamicQuery[0].Description = "Customers w/ No SO";
            qds.DynamicQuery[0].ExportID = jobName;
            qds.DynamicQuery[0].IsShared = true;
            qds.DynamicQuery[0].GlobalQuery = true;
            dqBusObj.Update(qds);
            dqBusObj.AddQueryTable(qds, jobName, "Customer", false);
            dqBusObj.Update(qds);
            dqBusObj.GetNewWhereItem(qds, jobName, "Customer");
            qds.QueryWhereItem[0].FieldName = "CustomerType";
            qds.QueryWhereItem[0].CompOp = "=";
            qds.QueryWhereItem[0].RValue = customertype;
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "Customer", "CustID");
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "Customer", "Name");
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "Customer", "CustNum");
            dqBusObj.Update(qds);
            qds.SelectedField[0].FieldLabel = "Cust ID";
            qds.SelectedField[1].FieldLabel = "Name";
            qds.SelectedField[2].FieldLabel = "Cust Num";
            dqBusObj.Update(qds);
            bool outbool;
            dqBusObj.SaveQuery(jobName, out outbool);
        }

        private void BuildCustomerOrdersQuery(DynamicQuery dqBusObj, string jobName, string custnum, DateTime period)
        {
            QueryDesignDataSet qds = new QueryDesignDataSet();
            dqBusObj.GetNewQuery(qds);
            qds.DynamicQuery[0].Description = "Customers w/ No SO";
            qds.DynamicQuery[0].ExportID = jobName;
            qds.DynamicQuery[0].IsShared = true;
            qds.DynamicQuery[0].GlobalQuery = true;
            dqBusObj.Update(qds);
            dqBusObj.AddQueryTable(qds, jobName, "OrderHed", false);
            dqBusObj.Update(qds);
            dqBusObj.GetNewWhereItem(qds, jobName, "OrderHed");
            qds.QueryWhereItem[0].FieldName = "OrderDate";
            qds.QueryWhereItem[0].CompOp = ">";
            qds.QueryWhereItem[0].RValueDate = period;
            dqBusObj.Update(qds);
            dqBusObj.GetNewWhereItem(qds, jobName, "OrderHed");
            qds.QueryWhereItem[1].FieldName = "CustNum";
            qds.QueryWhereItem[1].CompOp = "=";
            qds.QueryWhereItem[1].RValueNumber = custnum;
            qds.QueryWhereItem[1].AndOr = "and";
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "OrderHed", "OrderNum");
            dqBusObj.Update(qds);
            qds.SelectedField[0].FieldLabel = "OrderNum";
            dqBusObj.Update(qds);
            bool outbool;
            dqBusObj.SaveQuery(jobName, out outbool);
        }*/

        private System.Data.DataTable CustomersWNoSO(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetCustomersWithNoOrdersSince @Company, @CustomerType, @OrderDate");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("CustomerType", dc.SelectedValues["Customer Type"]);
                command.Parameters.AddWithValue("OrderDate", DateTime.Today.AddMonths(-1 * Int32.Parse(dc.SelectedValues["Months Period"])));
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Cust ID"] = row[0];
                    newRow["Name"] = row[1];
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Customer Info

        private System.Data.DataTable GetCustomerList(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetCustomerList @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Cust ID"] = row[0];
                    newRow["Name"] = row[1];
                    newRow["City"] = row[2];
                    newRow["State"] = row[3];
                    newRow["Zip"] = row[4];
                    newRow["Country"] = row[5];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void PrintCustomerInfo(object state)
        {
            object[] args = state as object[];
            try
            {
                DataGridViewSelectedRowCollection selectedRows = args[1] as DataGridViewSelectedRowCollection;

                DataSet ds = null;
                foreach (DataGridViewRow row in selectedRows)
                {
                    SqlCommand command = new SqlCommand("exec [dbo].sp_GetCustomerDetail @Company, @CustID");
                    command.Parameters.AddWithValue("Company", session.CompanyID);
                    command.Parameters.AddWithValue("CustID", row.Cells[0].Value);
                    DataSet this_ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                    if (ds == null)
                        ds = this_ds;
                    else
                    {
                        foreach (DataRow dr in this_ds.Tables[0].Rows)
                        {
                            ds.Tables[0].ImportRow(dr);
                        }
                    }
                }

                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    for (int x = 0; x < pd.PrinterSettings.Copies; x++)
                    {
                        LocalReport report = new LocalReport();
                        report.ReportPath = @"CustomerInfo.rdlc";

                        report.DataSources.Add(new ReportDataSource("DataSet", ds.Tables[0]));
                        report.SubreportProcessing += new SubreportProcessingEventHandler(report_SubreportProcessing);

                        ReportPrintDocument rpd = new ReportPrintDocument(report);
                        rpd.PrinterSettings = pd.PrinterSettings;
                        rpd.Print();
                    }
                }
                Modules[args[0].ToString()].State = Module.ModuleState.Loaded;
                DrawTaskPage(new string[] { args[0].ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void report_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "ShipTo")
            {
                SqlCommand command = new SqlCommand("exec [dbo].sp_GetCustomerDetailShipTos @Company, @Custnum");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Custnum", Int32.Parse(e.Parameters[0].Values[0].ToString()));
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);
                e.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
            }
            else if (e.ReportPath == "OrderHistory")
            {
                SqlCommand command = new SqlCommand("exec [dbo].sp_GetcustomerDetailOrders @Company, @Custnum");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Custnum", Int32.Parse(e.Parameters[0].Values[0].ToString()));
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);
                e.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
            }
        }

        #endregion

        #region Sales Rep Mgmt

        private System.Data.DataTable GetSalesRepMgmt(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetSalesRepMgmt @Company, @CustIDs");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("CustIDs", dc.SelectedValues["Cust IDs (comma seperated)"]);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Cust ID"] = row[0].ToString();
                    newRow["Customer"] = row[1].ToString();
                    newRow["Sales Rep Code"] = row[2].ToString();
                    newRow["Message"] = row[3].ToString();
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        private void SaveSalesRepMgmt(object state)
        {
            try
            {
                lock (Modules["Sales Rep Mgmt"].Data)
                {
                    CustomerImpl customer = WCFServiceSupport.CreateImpl<CustomerImpl>(session, CustomerImpl.UriPath);

                    foreach (DataRow row in Modules["Sales Rep Mgmt"].Data.Rows)
                    {
                        CustomerDataSet ds = customer.GetByCustID(row[0].ToString(), true);
                        string salesrepcode = row[2].ToString();
                        ((CustomerDataSet.CustomerRow)ds.Customer.Rows[0]).SalesRepCode = salesrepcode;
                        customer.Update(ds);
                        foreach (CustomerDataSet.ShipToRow shipto in ds.ShipTo.Rows)
                        {
                            if (!String.IsNullOrEmpty(shipto.ShipToNum))
                                shipto.SalesRepCode = salesrepcode;
                        }
                        customer.Update(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SaveSalesRepMgmt - " + ex.Message);
            }
            RefreshTab(new string[] { "Sales Rep Mgmt" });
        }

        #endregion

        #region Job Mismatched Dates - Ported

        private System.Data.DataTable GetJobsMismatchedOrderDates(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].[sp_GetJobOrder_DateMismatch]");
                /*command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("CustIDs", dc.SelectedValues["Cust IDs (comma seperated)"]);*/
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Order Shop Cap"] = DateTime.Parse(row[0].ToString()).ToShortDateString();
                    newRow["Job Req Due Date"] = DateTime.Parse(row[1].ToString()).ToShortDateString();
                    newRow["Order #"] = row[2];
                    newRow["Order Line #"] = row[3];
                    newRow["Order Release #"] = row[4];
                    newRow["Job #"] = row[5];
                    dt.Rows.Add(newRow);
                    break;
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void SaveJobsMismatchedOrderDates(object state)
        {
            try
            {
                lock (Modules["Edit Job/Order Dates"].Data)
                {
                    JobEntryImpl jeBusObj = WCFServiceSupport.CreateImpl<JobEntryImpl>(session, JobEntryImpl.UriPath);
                    SalesOrderImpl soBusObj = WCFServiceSupport.CreateImpl<SalesOrderImpl>(session, SalesOrderImpl.UriPath);

                    foreach (DataRow row in Modules["Edit Job/Order Dates"].Data.Rows)
                    {
                        JobEntryDataSet jeds = jeBusObj.GetByID(row[5].ToString());
                        SalesOrderDataSet sods = soBusObj.GetByID(Int32.Parse(row[2].ToString()));

                        foreach (SalesOrderDataSet.OrderRelRow rel in sods.OrderRel.Rows)
                        {
                            if (rel.OrderLine == Int32.Parse(row[3].ToString()) && rel.OrderRelNum == Int32.Parse(row[4].ToString()))
                                rel.ReqDate = DateTime.Parse(row[0].ToString());
                        }

                        jeds.JobHead[0].JobReleased = false;
                        jeBusObj.ChangeJobHeadJobReleased(jeds);
                        jeds.JobHead[0].JobEngineered = false;
                        jeBusObj.ChangeJobHeadJobEngineered(jeds);
                        jeBusObj.Update(jeds);

                        jeds.JobHead[0].ReqDueDate = DateTime.Parse(row[1].ToString());
                        jeds.JobHead[0].StartDate = DateTime.Parse(row[1].ToString());
                        jeds.JobHead[0].DueDate = DateTime.Parse(row[1].ToString());

                        jeBusObj.Update(jeds);

                        jeds.JobHead[0].JobEngineered = true;
                        jeBusObj.ChangeJobHeadJobEngineered(jeds);
                        jeds.JobHead[0].JobReleased = true;
                        jeBusObj.ChangeJobHeadJobReleased(jeds);
                        jeds.JobHead[0].ChangeDescription = "Automated change ReqDueDate, StartDate, DueDate to " + row[1].ToString();
                        jeBusObj.Update(jeds);

                        soBusObj.Update(sods);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            RefreshTab(new string[] { "Edit Job/Order Dates" });
        }

        #endregion

        #region Print Part Attachments

        private System.Data.DataTable GetJobPartsWithAttachments(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobsWithPartAttachments @Company, @Recurse");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Recurse", Int32.Parse(dc.SelectedValues["Search Depth"]));
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Job #"] = row[0];
                    newRow["Job Part #"] = row[1];
                    newRow["Job Status"] = System.Convert.ToInt32(row[2].ToString()) == 1 ? "Closed" : "Open";
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetMOMPartsWithAttachments(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetMOMsWithPartAttachments @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part #"] = row[0];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetFileAttachListForJob(List<GridColumn> columns, string jobnum, string partnum)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobPartAttachments @Company, @Job, @Recurse");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Job", jobnum);
                command.Parameters.AddWithValue("Recurse", Int32.Parse(dc.SelectedValues["Search Depth"]));
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                List<string> files = new List<string>();

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part #"] = row[0];
                    newRow["Assembly Seq"] = row[1];
                    newRow["Operation"] = row[2];
                    newRow["Mtl Seq"] = row[3];
                    newRow["Filename"] = row[4];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetFileAttachListForMOM(List<GridColumn> columns, string partnum)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("select * from [dbo].fn_AttachmentsForMOM(@Company, @Part) order by Opr asc, Partnum asc");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Part", partnum);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                List<string> files = new List<string>();

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //                    if (!files.Contains(row[1].ToString()))
                    //                  {
                    DataRow newRow = dt.NewRow();
                    newRow["Part #"] = row[0];
                    newRow["Operation"] = row[1];
                    newRow["Filename"] = row[2];
                    dt.Rows.Add(newRow);
                    //                    files.Add(row[1].ToString());
                    //              }
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void PrintPartAttachments(object state)
        {
            object[] args = state as object[];
            try
            {
                DataGridViewSelectedRowCollection selectedRows = args[1] as DataGridViewSelectedRowCollection;
                List<string> files = new List<string>();
                foreach (DataGridViewRow row in selectedRows)
                    files.Insert(0, row.Cells[4].Value.ToString());

                List<string> swFiles = new List<string>();
                List<string> pdfFiles = new List<string>();
                swView = new SolidWorksViewer();

                foreach (string file in files)
                {
                    string ext = file.Substring(file.LastIndexOf('.') + 1);
                    switch (ext.ToLower())
                    {
                        case "pdf":
                            if (!pdfFiles.Contains(file))
                                pdfFiles.Add(file);
                            break;
                        case "sldprt":
                        case "slddrw":
                        case "edrw":
                        case "dwg":
                        case "dxf":
                            if (!swFiles.Contains(file))
                                swFiles.Add(file);
                            break;
                        case "bak":
                            MessageBox.Show("BAK File \"" + file + "\" can not be printed.  Remove the .BAK file attachment and replace with the original file to print", "Print Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        default:
                            MessageBox.Show("Unsupported file extension: " + ext, "Can't print file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }

                if (printDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string printer = printDialog1.PrinterSettings.PrinterName;

                    if (swFiles.Count > 0)
                    {
                        swView.printerName = printer;
                        swView.filesToPrint = swFiles;
                        swView.ShowDialog();
                    }
                    if (pdfFiles.Count > 0)
                    {
                        string AcroLocation = GetKeyValue("AcrobatLocation");
                        if (String.IsNullOrEmpty(AcroLocation))
                        {
                            SearchForAcrobat sfa = new SearchForAcrobat();
                            if (sfa.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                AcroLocation = sfa.AcrobatLocation;
                                SetKeyValue("AcrobatLocation", AcroLocation);
                            }
                        }

                        if (!String.IsNullOrEmpty(AcroLocation))
                        {
                            foreach (string file in pdfFiles)
                            {
                                PdfFilePrinter pdfprinter = new PdfFilePrinter(file, printer);
                                PdfFilePrinter.AdobeReaderPath = AcroLocation;
                                pdfprinter.Print();
                            }
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {
                if (ex.Message == "The system cannot find the file specified")
                {
                    MessageBox.Show("Could not find Adobe Acrobat program at the previous location.  Please reprint and the location will be updated", "Could not find Acrobat program", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetKeyValue("AcrobatLocation", "");
                }
                else
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Modules[args[0].ToString()].State = Module.ModuleState.Loaded;
            DrawTaskPage(new string[] { args[0].ToString() });
        }

        #endregion

        #region Get Parts Below Req Qty

        private System.Data.DataTable GetJobsMissingParts(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetOpenJobs @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Job #"] = row[0];
                    newRow["Job Part #"] = row[1];
                    newRow["Job Part Description"] = row[2];
                    newRow["Visibility"] = row[3];
                    newRow["Change Visibility"] = row[3].ToString() == "VISIBLE" ? "Hide" : "Show";
                    newRow["Backflush On"] = "Turn On";
                    newRow["Backflush Off"] = "Turn Off";
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetJobMissingPartsDetails(List<GridColumn> columns, string jobnum)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobsMissingPartsv2 @Company, @Jobnum");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Jobnum", jobnum);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Job #"] = row[0];
                    newRow["Job Partnum"] = row[1];
                    newRow["Job Part Description"] = row[3];
                    newRow["Assembly Seq"] = row[4];
                    newRow["Operation"] = row[5];
                    newRow["Mtl Seq"] = row[6];
                    newRow["Mtl Partnum"] = row[7];
                    newRow["Mtl Part Type"] = row[8];
                    newRow["Mtl Normally Purchased"] = row[9];
                    newRow["Mtl Revision"] = row[10];
                    newRow["Mtl Description"] = row[11];
                    newRow["Below Req Qty"] = row[12].ToString() == "1" ? "Yes" : "No";
                    newRow["Mtl Warehouse Code"] = row[13];
                    newRow["Mtl On Hand Qty"] = row[14];
                    newRow["Mtl Non Inventory Qty"] = row[15];
                    newRow["Mtl Total SO Demand"] = row[16];
                    newRow["Mtl Total Job Demand"] = row[17];
                    newRow["Mtl Incoming PO"] = row[18];
                    newRow["Mtl Incoming Job"] = row[19];
                    newRow["Mtl Comment"] = row[20];
                    newRow["Visibility"] = row[21];
                    newRow["Change Visibility"] = row[21].ToString() == "VISIBLE" ? "Hide" : "Show";
                    newRow["Mtl Bin"] = row[22];
                    newRow["Mtl Pull Qty"] = row[23];
                    newRow["Backflush"] = row[24];
                    newRow["Last Backflush"] = row[25].ToString() == "True" ? "1" : "0";
                    newRow["Qty Bearing"] = row[26];
                    newRow["Direct"] = row[27];
                    newRow["Issued Qty"] = row[28];

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetPartsBelowReqQty(List<GridColumn> columns, string jobnum)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetPartsBelowReqQty @Company, @Jobnum");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Jobnum", jobnum);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Partnum"] = row[0];
                    newRow["Part Type"] = row[1];
                    newRow["Normally Purchased"] = row[12];
                    newRow["Revision"] = row[2];
                    newRow["Warehouse Code"] = row[3];
                    newRow["Description"] = row[4];
                    newRow["On Hand Qty"] = row[5];
                    newRow["Non Inventory Qty"] = row[6];
                    newRow["Total SO Demand"] = row[7];
                    newRow["Total Job Demand"] = row[8];
                    newRow["Incoming PO"] = row[9];
                    newRow["Incoming Job"] = row[10];
                    newRow["Visibility"] = row[11];
                    newRow["Change Visibility"] = row[11].ToString() == "VISIBLE" ? "Hide" : "Show";
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        private System.Data.DataTable GetDetailsForPartBelowReqQty(List<GridColumn> columns, string partnum, string revision, string warehousecode)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetIncomingQtyForPart @Company, @Partnum, @Revision, @Warehousecode");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Partnum", partnum);
                command.Parameters.AddWithValue("Revision", revision);
                command.Parameters.AddWithValue("Warehousecode", warehousecode);

                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Partnum"] = row[0];
                    newRow["Revision"] = row[1];
                    newRow["Warehouse"] = row[2];
                    newRow["PO #"] = row[3];
                    newRow["PO Source"] = row[4];
                    newRow["PO Qty"] = row[5];
                    newRow["PO Due Date"] = row[6];
                    newRow["Job #"] = row[7];
                    newRow["Job Source"] = row[8];
                    newRow["Job Qty"] = row[9];
                    newRow["Job Due Date"] = row[10];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable FilterPartsBelowReqQty(string filter, System.Data.DataTable data)
        {
            try
            {
                DataTable result = data.Copy();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    if (result.Rows[i]["Part Type"].ToString() != filter)
                    {
                        result.Rows.Remove(result.Rows[i]);
                        i--;
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        private void ChangeDisplaySettings(string jobnum, string assembly, string operation, string ordernum, string orderline, string partnum, Boolean add)
        {
            try
            {
                SqlCommand command = new SqlCommand("exec [dbo].sp_MissingPartsListSuppressActionv2 @Company, @Jobnum, @AssemblySeq, @Operation, @Ordernum, @Orderline, @Partnum, @Action");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                if (jobnum.Length > 0)
                    command.Parameters.AddWithValue("Jobnum", jobnum);
                else
                    command.Parameters.AddWithValue("Jobnum", DBNull.Value);
                if (assembly.Length > 0)
                    command.Parameters.AddWithValue("AssemblySeq", assembly);
                else
                    command.Parameters.AddWithValue("AssemblySeq", DBNull.Value);
                if (operation.Length > 0)
                    command.Parameters.AddWithValue("Operation", operation);
                else
                    command.Parameters.AddWithValue("Operation", DBNull.Value);
                if (ordernum.Length > 0)
                    command.Parameters.AddWithValue("Ordernum", Int32.Parse(ordernum));
                else
                    command.Parameters.AddWithValue("Ordernum", DBNull.Value);
                if (orderline.Length > 0)
                    command.Parameters.AddWithValue("Orderline", Int32.Parse(orderline));
                else
                    command.Parameters.AddWithValue("Orderline", DBNull.Value);
                command.Parameters.AddWithValue("Partnum", partnum);
                command.Parameters.AddWithValue("Action", add ? 1 : 0);

                SQLAccess.NonQuery(Server, Database, "", "", command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EnterMissingPartComment(string jobnum, string assembly, string operation, string partnum, string comment)
        {
            try
            {
                SqlCommand command = new SqlCommand("exec [dbo].sp_UpdateMissingPartComment @Company, @Jobnum, @AssemblySeq, @Operation, @Partnum, @Comment");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Jobnum", jobnum);
                command.Parameters.AddWithValue("AssemblySeq", assembly);
                command.Parameters.AddWithValue("Operation", operation);
                command.Parameters.AddWithValue("Partnum", partnum);
                command.Parameters.AddWithValue("Comment", comment);

                SQLAccess.NonQuery(Server, Database, "", "", command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrintJobMissingPartsList(object state)
        {
            object[] args = state as object[];
            try
            {
                string ReportTitle = args[1] as string;
                DataGridViewSelectedRowCollection selectedRows = args[2] as DataGridViewSelectedRowCollection;
                string parttype = args[3] as string;
                string visibility = args[4] as string;
                StringBuilder builder = new StringBuilder();
                builder.Append("<html><title>" + ReportTitle + "</title><body>");
                builder.Append("<h3>" + ReportTitle + "</h3>");
                builder.Append("<h5>" + selectedRows[0].Cells[12].Value + " - " + selectedRows[0].Cells[13].Value + "</h5>");
                builder.Append("<h5>Part Types Shown: " + parttype + ", Record Visibility: " + visibility + "</h5>");
                builder.Append("<table><tbody>");
                StringBuilder data = new StringBuilder();
                foreach (DataGridViewRow row in selectedRows)
                {
                    StringBuilder details = new StringBuilder();
                    if (!String.IsNullOrEmpty(row.Cells[21].Value.ToString()) || !String.IsNullOrEmpty(row.Cells[22].Value.ToString()))
                    {
                        SqlCommand command = new SqlCommand("exec [dbo].sp_GetIncomingQtyForPart @Company, @Partnum, @Revision, @Warehousecode");
                        command.Parameters.AddWithValue("Company", session.CompanyID);
                        command.Parameters.AddWithValue("Partnum", row.Cells[7].Value.ToString());
                        command.Parameters.AddWithValue("Revision", row.Cells[14].Value.ToString());
                        command.Parameters.AddWithValue("Warehousecode", row.Cells[15].Value.ToString());

                        DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                        if (ds.Tables.Count > 0)
                        {
                            foreach (DataRow drow in ds.Tables[0].Rows)
                            {
                                if (!String.IsNullOrEmpty(drow[3].ToString()))
                                    details.Append("<tr><td colspan='1'><td>Incoming PO #" + drow[3].ToString() + ", Source: " + drow[4].ToString() + ", Qty: " + drow[5].ToString() + ", Due: " + (String.IsNullOrEmpty(drow[6].ToString()) ? "" : DateTime.Parse(drow[6].ToString()).ToShortDateString()) + "</td></tr>");
                                else
                                    details.Append("<tr><td colspan='1'><td>Incoming Job #" + drow[7].ToString() + ", Source: " + drow[8].ToString() + ", Qty: " + drow[9].ToString() + ", Due: " + (String.IsNullOrEmpty(drow[10].ToString()) ? "" : DateTime.Parse(drow[10].ToString()).ToShortDateString()) + "</td></tr>");
                            }
                        }
                    }

                    data.Insert(0,
                        "<tr><td>" +
                        "<table style='font-size:xx-small;'>" +
                        "<tbody>" +
                        "<tr><td>Part #</td><td>" + row.Cells[7].Value + ", Rev #" + row.Cells[14].Value + ", Type: " + row.Cells[9].Value + ", Normally Purchased: " + row.Cells[10].Value + ", Warehouse: " + row.Cells[15].Value + ", Bin: " + row.Cells[6].Value + "</td></tr>" +
                        "<tr><td colspan='1'></td><td colspan='2'>" + row.Cells[16].Value + "</td></tr>" +
                        "<tr><td colspan='1'></td><td>Job Qty: " + row.Cells[8].Value + ", On Hand Qty: " + row.Cells[17].Value + ", Non Inv Qty: " + row.Cells[18].Value + ", SO Demand: " + row.Cells[19].Value + ", Job Demand: " + row.Cells[20].Value + "</td></tr>" +
                        "<tr><td colspan='1'></td><td>Below Req Qty: " + row.Cells[23].Value + ", Incoming PO Qty: " + (String.IsNullOrEmpty(row.Cells[21].Value.ToString()) ? "0" : row.Cells[21].Value) + ", Incoming Job Qty: " + (String.IsNullOrEmpty(row.Cells[22].Value.ToString()) ? "0" : row.Cells[22].Value) + "</td></tr>" +
                        "<tr><td colspan='1'></td><td>Comments: " + row.Cells[11].Value + "</td></tr>" +
                        details.ToString() +
                        "</tbody>" +
                        "</table>" +
                        "</td></tr>");
                }
                builder.Append(data.ToString());
                builder.Append("</tbody></table>");
                builder.Append("</body></html>");

                webBrowser1.DocumentText = builder.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangeBackFlush(string jobnum, int assemblyseq, int mtlseq, bool state)
        {
            try
            {
                JobEntryImpl je = WCFServiceSupport.CreateImpl<JobEntryImpl>(session, JobEntryImpl.UriPath);

                JobEntryDataSet ds = je.GetByID(jobnum);

                bool released = ds.JobHead[0].JobReleased;
                bool engineered = ds.JobHead[0].JobEngineered;

                if (released)
                    ds.JobHead[0].JobReleased = false;
                if (engineered)
                    ds.JobHead[0].JobEngineered = false;
                ds.JobHead[0].ChangeDescription = "TCG Dashboard - Update backflush job #" + jobnum + ", assembly seq " + assemblyseq.ToString() + ", mtlseq " + mtlseq.ToString() + ", backflush to " + state.ToString();

                je.Update(ds);

                foreach (JobEntryDataSet.JobMtlRow row in ds.JobMtl)
                {
                    if (row.AssemblySeq == assemblyseq && row.MtlSeq == mtlseq && row.BackFlush != state)
                    {
                        row.BackFlush = state;
                        je.ChangeJobMtlBackflush(ds);
                        je.Update(ds);
                    }
                }

                if (engineered)
                {
                    ds.JobHead[0].JobEngineered = true;
                    je.ChangeJobHeadJobEngineered(ds);
                }
                if (released)
                {
                    ds.JobHead[0].JobReleased = true;
                    je.ChangeJobHeadJobReleased(ds);
                }

                je.Update(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.Source + " - " + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangeBackFlush(string jobnum, bool state)
        {
            try
            {
                JobEntryImpl je = WCFServiceSupport.CreateImpl<JobEntryImpl>(session, JobEntryImpl.UriPath);

                JobEntryDataSet ds = je.GetByID(jobnum);

                bool released = ds.JobHead[0].JobReleased;
                bool engineered = ds.JobHead[0].JobEngineered;

                if (released)
                    ds.JobHead[0].JobReleased = false;
                if (engineered)
                    ds.JobHead[0].JobEngineered = false;

                ds.JobHead[0].ChangeDescription = "TCG Dashboard - Update backflush job #" + jobnum + " backflush to " + state.ToString();
                je.Update(ds);

                foreach (JobEntryDataSet.JobMtlRow row in ds.JobMtl)
                {
                    if (row.BackFlush != state)
                    {
                        row.BackFlush = state;
                        je.ChangeJobMtlBackflush(ds);
                        je.Update(ds);
                    }
                }

                if (engineered)
                {
                    ds.JobHead[0].JobEngineered = true;
                    je.ChangeJobHeadJobEngineered(ds);
                }
                if (released)
                {
                    ds.JobHead[0].JobReleased = true;
                    je.ChangeJobHeadJobReleased(ds);
                }

                je.Update(ds);

                SqlCommand command = new SqlCommand("exec [db].sp_UpdateMissingPartBackflush @Company, @Jobnum, @Backflush");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Jobnum", jobnum);
                command.Parameters.AddWithValue("Backflush", state);
                SQLAccess.NonQuery(Server, Database, "", "", command);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.Source + " - " + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IssueMaterial(string company, string jobnum, int assemblyseq, int mtlseq, decimal qty)
        {
            try
            {
                /*                IssueReturnImpl ir = WCFServiceSupport.CreateImpl<IssueReturnImpl>(session, IssueReturnImpl.UriPath);
                                SelectedJobAsmblDataSet jds = new SelectedJobAsmblDataSet();
                                SelectedJobAsmblDataSet.SelectedJobAsmblRow jrow = jds.SelectedJobAsmbl.NewSelectedJobAsmblRow();
                                jrow.JobNum = jobnum;
                                jrow.Company = company;
                                jrow.AssemblySeq = assemblyseq;
                                jds.SelectedJobAsmbl.AddSelectedJobAsmblRow(jrow);
                                string pcmessage;
                                IssueReturnDataSet ds = ir.GetNewJobAsmblMultiple("STK-MTL", "?", jds, out pcmessage);

                                IssueReturnDataSet.IssueReturnRow irrow = ds.IssueReturn.Rows[0] as IssueReturnDataSet.IssueReturnRow;
                                irrow.ToJobSeq = mtlseq;
                                ir.OnChangeToJobSeq(ds, out pcmessage);
                                irrow.TranQty = qty;
                                ir.OnChangeTranQty(qty, ds);

                                string negqty;
                                ir.NegativeInventoryTest(irrow.PartNum, irrow.FromWarehouseCode, irrow.FromBinNum, irrow.LotNum, irrow.DimCode, irrow.DimConvFactor, irrow.TranQty, out negqty, out pcmessage);
                                ir.PerformMaterialMovement(!String.IsNullOrEmpty(negqty), ds, out pcmessage);*/
                throw new Exception("Not implemented");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.Source + " - " + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReturnMaterial(string company, string jobnum, int assemblyseq, int mtlseq, decimal qty)
        {
            try
            {
                /*IssueReturnImpl ir = WCFServiceSupport.CreateImpl<IssueReturnImpl>(session, IssueReturnImpl.UriPath);
                SelectedJobAsmblDataSet jds = new SelectedJobAsmblDataSet();
                SelectedJobAsmblDataSet.SelectedJobAsmblRow jrow = jds.SelectedJobAsmbl.NewSelectedJobAsmblRow();
                jrow.JobNum = jobnum;
                jrow.Company = company;
                jrow.AssemblySeq = assemblyseq;
                jds.SelectedJobAsmbl.AddSelectedJobAsmblRow(jrow);
                string pcmessage;
                IssueReturnDataSet ds = ir.GetNewJobAsmblMultiple("MTL-STK", "?", jds, out pcmessage);

                IssueReturnDataSet.IssueReturnRow irrow = ds.IssueReturn.Rows[0] as IssueReturnDataSet.IssueReturnRow;
                irrow.FromJobSeq = mtlseq;
                ir.OnChangeFromJobSeq(ds, out pcmessage);
                irrow.TranQty = qty;
                ir.OnChangeTranQty(qty, ds);

                ir.PerformMaterialMovement(false, ds, out pcmessage);*/
                throw new Exception("Not Implemented");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.Source + " - " + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Get Order Parts Below Req Qty

        private System.Data.DataTable GetOrdersMissingParts(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetOrdersMissingParts @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Order #"] = row[0];
                    newRow["Order Line"] = row[1];
                    newRow["Order Rel #"] = row[2];
                    newRow["Req Date"] = row[3].ToString() == "" ? "" : DateTime.Parse(row[3].ToString()).ToShortDateString();
                    newRow["Partnum"] = row[4];
                    newRow["Revision"] = row[5];
                    newRow["Part Type"] = row[6];
                    newRow["Normally Purchased"] = row[8];
                    newRow["Description"] = row[7];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetOrderPartsBelowReqQty(List<GridColumn> columns, int ordernum, int orderline, int orderrelnum)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetPartsBelowReqQty_ForOrders @Company, @Ordernum, @Orderline, @Orderrelnum");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Ordernum", ordernum);
                command.Parameters.AddWithValue("Orderline", orderline);
                command.Parameters.AddWithValue("Orderrelnum", orderrelnum);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Partnum"] = row[0];
                    newRow["Revision"] = row[1];
                    newRow["Warehouse Code"] = row[2];
                    newRow["Description"] = row[3];
                    newRow["On Hand Qty"] = row[4];
                    newRow["Non Inventory Qty"] = row[5];
                    newRow["Total SO Demand"] = row[6];
                    newRow["Total Job Demand"] = row[7];
                    newRow["Incoming PO"] = row[8];
                    newRow["Incoming Job"] = row[9];
                    newRow["Visibility"] = row[10];
                    newRow["Change Visibility"] = row[10].ToString() == "VISIBLE" ? "Hide" : "Show";
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Get Job Manuals

        private System.Data.DataTable GetJobManuals(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobs @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Job #"] = row[0];
                    newRow["Job Status"] = System.Convert.ToInt32(row[1].ToString()) == 1 ? "Closed" : "Open";
                    newRow["Part"] = row[2];
                    newRow["Description"] = row[4];
                    newRow["Quantity"] = row[5];
                    newRow["Customer"] = row[6];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetJobManualPage1Details(List<GridColumn> columns, string jobnum)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobAssemblies @Company, @Jobnum");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Jobnum", jobnum);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Sub Number"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["Quantity"] = row[2];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetJobManualPage2Details(List<GridColumn> columns, string jobnum)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobAssemblyParts @Company, @Jobnum");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Jobnum", jobnum);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                string lastparthead = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    if (row[0].ToString() != lastparthead)
                    {
                        newRow["Sub Number"] = row[0];
                        newRow["Part Description"] = row[1];
                        lastparthead = row[0].ToString();
                        dt.Rows.Add(newRow);
                        DataRow secondRow = dt.NewRow();
                        secondRow["Part Description"] = row[3];
                        secondRow["Quantity"] = row[5];
                        secondRow["Part Number"] = row[4];
                        dt.Rows.Add(secondRow);
                    }
                    else
                    {
                        newRow["Part Description"] = row[3];
                        newRow["Quantity"] = row[5];
                        newRow["Part Number"] = row[4];
                        dt.Rows.Add(newRow);
                    }
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Job Assemblies With No Auto Rec - Ported

        private System.Data.DataTable GetAssembliesWithNoAutoRec(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobAssembliesNoAutoRec @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Job #"] = row[0];
                    newRow["Assembly Seq"] = row[1];
                    newRow["Auto Receive Opr"] = row[2];
                    newRow["Final Opr"] = row[3];
                    newRow["Highest Opr Seq"] = row[4];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Part Min Qty Alerts

        private System.Data.DataTable GetPartMinQtyAlerts(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_PartTran_Find_Min_To_Change @Company, @Plant, @PartClass");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                command.Parameters.AddWithValue("PartClass", dc.SelectedValues["Part Class"]);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["Plant"] = row[2];
                    newRow["On Hand Qty"] = row[3];
                    newRow["Min Qty"] = row[4];
                    newRow["Monthly Usage"] = row[5];
                    newRow["Difference"] = Decimal.Parse(row[6].ToString()) / 100.0m;
                    newRow["Avg Cost"] = row[7];
                    newRow["Months To Stock"] = 1;
                    newRow["New Minimum"] = row[5];
                    newRow["Total Cost"] = Decimal.Parse(row[7].ToString()) * Decimal.Parse(row[5].ToString());
                    newRow["Process"] = "Yes";

                    for (int i = 12; i < columns.Count; i++)
                        newRow[i] = 0;

                    for (int i = 8; i < ds.Tables[0].Columns.Count; i++)
                        newRow[ds.Tables[0].Columns[i].ColumnName] = row[i];

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void SavePartMinQty(object state)
        {
            List<DataGridViewRow> rows = state as List<DataGridViewRow>;

            try
            {
                PartImpl partBusObj = WCFServiceSupport.CreateImpl<PartImpl>(session, PartImpl.UriPath);
                foreach (DataGridViewRow row in rows)
                {
                    PartDataSet ds = partBusObj.GetByID(row.Cells["Part"].Value.ToString());

                    foreach (PartDataSet.PartPlantRow pp in ds.PartPlant.Rows)
                    {
                        if (pp.Plant == row.Cells["Plant"].Value.ToString())
                            pp.MinimumQty = Decimal.Parse(row.Cells["New Minimum"].Value.ToString());
                    }

                    partBusObj.Update(ds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Modules["Part Min Qty Alerts"].State = Module.ModuleState.Saved;
            RefreshTab(new string[] { "Part Min Qty Alerts" });
        }

        public Dictionary<string, string> GetPlants(string company)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPlantsForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[0].ToString(), row[0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Dictionary<string, string> GetPlants()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPlantsForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", session.CompanyID);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[0].ToString(), row[0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Dictionary<string, string> GetPlantsForUser()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPlantsForUser @Userid");
            sqlCommand.Parameters.AddWithValue("Userid", session.UserID);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[0].ToString(), row[0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Dictionary<string, string> GetPartClasses()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPartClassesForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", session.CompanyID);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", sqlCommand);

                result.Add("All", "%");
                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[1].ToString().Trim(), row[0].ToString().Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Part Min Qty Report

        private System.Data.DataTable GetPartMinQtyReport(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_PartTran_MinQtyReport @Company, @Plant, @PartClass");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                command.Parameters.AddWithValue("PartClass", dc.SelectedValues["Part Class"]);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["Part Class"] = row[2];
                    newRow["Plant"] = row[3];
                    newRow["On Hand Qty"] = row[4];
                    newRow["Min Qty"] = row[5];
                    newRow["Monthly Usage"] = row[6];
                    newRow["Difference"] = Decimal.Parse(row[7].ToString()) / 100.0m;
                    try
                    {
                        newRow["Avg Cost"] = Decimal.Parse(row[8].ToString()) * Decimal.Parse(row[4].ToString());
                    }
                    catch (Exception ex)
                    {
                        newRow["Avg Cost"] = 0;
                    }

                    for (int i = 9; i < columns.Count; i++)
                        newRow[i] = 0;

                    for (int i = 9; i < ds.Tables[0].Columns.Count; i++)
                        newRow[ds.Tables[0].Columns[i].ColumnName] = row[i];

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Part Min Qty Template

        private System.Data.DataTable GetPartMinQtyTemplate(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_PartTran_MinQtyReport @Company, @Plant, @PartClass");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                command.Parameters.AddWithValue("PartClass", dc.SelectedValues["Part Class"]);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["On Hand Qty"] = row[4];
                    try
                    {
                        newRow["Unit Cost"] = Decimal.Parse(row[8].ToString());
                    }
                    catch (Exception ex)
                    {
                        newRow["Unit Cost"] = 0;
                    }

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Purchase Receipt Summaries

        private DataTable GetPurchaseReceiptSummaries(List<GridColumn> columns, DateTime from, DateTime to)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_ReceiptTotalsByGLAccount @Company, @FromDate, @ToDate");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("FromDate", from);
                command.Parameters.AddWithValue("ToDate", to.AddDays(1).AddSeconds(-1));
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                decimal total = 0;
                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["G/L Account"] = row[1];
                    newRow["G/L Account Description"] = row[2];
                    newRow["Total Receipt $"] = row[0];
                    total += Decimal.Parse(row[0].ToString());
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetPurchaseReceiptSummariesTotal(DataTable data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Total Receipt $", typeof(Decimal));
            dt.Columns["Total Receipt $"].ReadOnly = true;

            decimal total = 0;
            foreach (DataRow row in data.Rows)
                total += Decimal.Parse(row["Total Receipt $"].ToString());

            if (total > 0)
            {
                DataRow totalRow = dt.NewRow();
                totalRow["Total Receipt $"] = total;
                dt.Rows.Add(totalRow);
            }

            return dt;
        }

        private DataTable GetPurchaseReceiptSummariesForGLAccount(List<GridColumn> columns, string glaccount, DateTime from, DateTime to)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_ReceiptTotalsByVendorForGLAccount @Company, @GLAccount, @FromDate, @ToDate");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("GLAccount", glaccount);
                command.Parameters.AddWithValue("FromDate", from);
                command.Parameters.AddWithValue("ToDate", to.AddDays(1).AddSeconds(-1));
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["G/L Account"] = row[3];
                    newRow["Vendor"] = row[1];
                    newRow["Total Receipt $"] = row[0];
                    newRow["Vendor #"] = row[2];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetPurchaseReceiptsForGLAccountAndVendor(List<GridColumn> columns, string glaccount, string vendornum, DateTime from, DateTime to)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_ReceiptsByGLAccountAndVendor @Company, @GLAccount, @Vendornum, @FromDate, @ToDate");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("GLAccount", glaccount);
                command.Parameters.AddWithValue("Vendornum", Int32.Parse(vendornum));
                command.Parameters.AddWithValue("FromDate", from);
                command.Parameters.AddWithValue("ToDate", to.AddDays(1).AddSeconds(-1));
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Invoice #"] = row[0];
                    newRow["Invoice Line"] = row[1];
                    newRow["Part"] = row[2];
                    newRow["Description"] = row[3];
                    newRow["Our Qty Received"] = row[4];
                    newRow["Our Unit Cost"] = row[5];
                    newRow["Our Total Cost"] = row[6];
                    newRow["Job #"] = row[7];
                    newRow["PO #"] = row[8];
                    newRow["PO Line"] = row[9];
                    newRow["PO Rel #"] = row[10];
                    newRow["Received To"] = row[11];
                    newRow["Receipt Date"] = row[12];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region JobMtl w/ IssuedQty 25% >= ReqQty - Ported

        private DataTable GetJobMtlsIssuedQty25PerGTReqQty(List<GridColumn> columns, DateTime from, DateTime to)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetJobMtlsWithIssuedQtyGTReqQty @Company, @Plant, @PartClass, @FromDate, @ToDate");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                command.Parameters.AddWithValue("PartClass", dc.SelectedValues["Part Class"]);
                command.Parameters.AddWithValue("FromDate", from);
                command.Parameters.AddWithValue("ToDate", to.AddDays(1).AddSeconds(-1));

                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Job Status"] = row[0].ToString() == "0" ? "Open" : "Closed";
                    newRow["Created Date"] = row[1];
                    newRow["Req Due Date"] = row[2];
                    newRow["Plant"] = row[3];
                    newRow["Job #"] = row[4];
                    newRow["Job Part #"] = row[5];
                    newRow["Assembly"] = row[6];
                    newRow["Operation"] = row[7];
                    newRow["Mtl Part #"] = row[8];
                    newRow["Req Qty"] = Decimal.Parse(row[9].ToString());
                    newRow["Issued Qty"] = Decimal.Parse(row[10].ToString());
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Outsourced Materials

        private DataTable GetOutsourcedMaterialManagement(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetManufacturedParts @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);

                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part #"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["Normally Purchased"] = row[2];
                    newRow["Original Normally Purchased"] = row[3];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void SaveOutsourcedMaterialManagement(object state)
        {
            try
            {
                /*
                Part partBusObj = new Part(session.ConnectionPool);

                foreach (DataRow row in Modules["Outsourced Material Management"].Data.Rows)
                {
                    if (row["Normally Purchased"].ToString() != row["Original Normally Purchased"].ToString())
                    {
                        PartDataSet ds = partBusObj.GetByID(row["Part #"].ToString());

                        if (row["Normally Purchased"].ToString() == "Yes")
                        {
                            foreach (PartDataSet.PartRow p in ds.Part.Rows)
                            {
                                if (p.TypeCode != "M" || p.CheckBox13 == false)
                                {
                                    p.TypeCode = "M";
                                    partBusObj.ChangePartTypeCode("M", ds);
                                    p.CheckBox13 = true;
                                }
                            }
                            partBusObj.Update(ds);
                            ds = partBusObj.GetByID(row["Part #"].ToString());
                            foreach (PartDataSet.PartPlantRow pp in ds.PartPlant.Rows)
                            {
                                if (pp.SourceType != "M")
                                {
                                    pp.SourceType = "M";
                                }
                            }
                            partBusObj.Update(ds);
                            ds = partBusObj.GetByID(row["Part #"].ToString());
                        }
                        else
                        {
                            foreach (PartDataSet.PartRow p in ds.Part.Rows)
                            {
                                if (p.TypeCode != "P" || p.CheckBox13 == true)
                                {
                                    p.TypeCode = "P";
                                    partBusObj.ChangePartTypeCode("P", ds);
                                    p.CheckBox13 = false;
                                }
                            }
                            partBusObj.Update(ds);
                            ds = partBusObj.GetByID(row["Part #"].ToString());
                            foreach (PartDataSet.PartPlantRow pp in ds.PartPlant.Rows)
                            {
                                if (pp.SourceType != "P")
                                {
                                    pp.SourceType = "P";
                                }
                            }
                            partBusObj.Update(ds);
                            ds = partBusObj.GetByID(row["Part #"].ToString());
                        }
                    }
                }*/
                throw new Exception("Not Implemented");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Modules["Outsourced Material Management"].State = Module.ModuleState.Saved;
            RefreshTab(new string[] { "Outsourced Material Management" });
        }

        #endregion

        #region Parts Fuzzy Lookup

        private DataTable GetPartsFuzzyLookup(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetPartsFuzzyMatch @Company, @Part");
                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Part", dc.SelectedValues["Partnum"]);

                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part #"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["Search Word"] = row[2];
                    newRow["Qty Bearing"] = row[3].ToString() == "1" ? "Yes" : "No";
                    newRow["Type"] = row[4];
                    newRow["Phantom BOM"] = row[5].ToString() == "1" ? "Yes" : "No";
                    newRow["Non-Stock Item"] = row[6].ToString() == "1" ? "Yes" : "No";
                    newRow["Group"] = row[7];
                    newRow["Class"] = row[8];
                    newRow["Invty U/M"] = row[9];
                    newRow["InActive"] = row[10].ToString() == "1" ? " Yes" : "No";

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Parts Over Ordered

        private DataTable GetPartsOverOrdered(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_PartsWithDemandPlusMaxGTOnHand @Company");
                command.Parameters.AddWithValue("Company", session.CompanyID);

                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part #"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["On Hand Qty"] = row[2];
                    newRow["Max Qty"] = row[3];
                    newRow["Demand"] = row[4];

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Part Time Phase

        private System.Data.DataTable GetPartTimePhase(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = null;
                if (dc.SelectedValues["PO Only"] == "Yes")
                    command = new SqlCommand("exec [dbo].sp_GetPartTimePhasePOOnly @Company, @Plant, @PartClass");
                else
                    command = new SqlCommand("exec [dbo].sp_GetPartTimePhase @Company, @Plant, @PartClass");

                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                command.Parameters.AddWithValue("PartClass", dc.SelectedValues["Part Class"]);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part #"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["Plant"] = row[2];

                    for (int i = 3; i < columns.Count; i++)
                        newRow[i] = 0;

                    for (int i = 3; i < ds.Tables[0].Columns.Count; i++)
                        newRow[ds.Tables[0].Columns[i].ColumnName] = row[i];

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Part Time Phase

        private System.Data.DataTable GetPartTimePhaseWithSuppliers(List<GridColumn> columns)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = null;
                command = new SqlCommand("exec [dbo].sp_GetPartTimePhaseVendorOnly @Company, @Plant, @PartClass");

                command.Parameters.AddWithValue("Company", session.CompanyID);
                command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                command.Parameters.AddWithValue("PartClass", dc.SelectedValues["Part Class"]);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable(columns);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part #"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["Plant"] = row[2];
                    newRow["Supplier"] = row[3];
                    newRow["On Hand Qty"] = row[4];
                    newRow["On Hand $"] = row[5];
                    newRow["Min Qty"] = row[6];
                    newRow["PO #"] = row[7];
                    newRow["PO Qty"] = row[8];
                    newRow["Due Date"] = row[9];



                    int running = 0;
                    int count = 0;

                    for (int i = 11; i < columns.Count; i++)
                        newRow[i] = 0;

                    for (int i = 11; i < ds.Tables[0].Columns.Count; i++)
                    {
                        running += Int32.Parse(row[i].ToString());
                        count++;
                        newRow[ds.Tables[0].Columns[i].ColumnName] = row[i];
                    }
                    newRow["Avg Monthly"] = ((int)Math.Round((double)(running / count))).ToString();

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region New Count Group

        private DataTable NewCountGroup(List<GridColumn> columns)
        {
            System.Data.DataTable dt = GenerateGridTable(columns);
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec [dbo].sp_GetPartsForABCCount @Plant, @ABC, @Limit, @BinFrom, @BinTo, @FuzzyPart, @FuzzyBin, @RepairDate");
                command.Parameters.AddWithValue("Plant", dc.SelectedValues["Plant"]);
                command.Parameters.AddWithValue("ABC", dc.SelectedValues["ABC Code"]);
                command.Parameters.AddWithValue("Limit", dc.SelectedValues["Limit"]);

                if (!String.IsNullOrEmpty(dc.SelectedValues["Bin From"]))
                    command.Parameters.AddWithValue("BinFrom", dc.SelectedValues["Bin From"].ToString());
                else
                    command.Parameters.AddWithValue("BinFrom", DBNull.Value);
                if (!String.IsNullOrEmpty(dc.SelectedValues["Bin To"]))
                    command.Parameters.AddWithValue("BinTo", dc.SelectedValues["Bin To"].ToString());
                else
                    command.Parameters.AddWithValue("BinTo", DBNull.Value);
                if (!String.IsNullOrEmpty(dc.SelectedValues["Fuzzy Part"]))
                    command.Parameters.AddWithValue("FuzzyPart", dc.SelectedValues["Fuzzy Part"].ToString());
                else
                    command.Parameters.AddWithValue("FuzzyPart", DBNull.Value);
                if (!String.IsNullOrEmpty(dc.SelectedValues["Fuzzy Bin"]))
                    command.Parameters.AddWithValue("FuzzyBin", dc.SelectedValues["Fuzzy Bin"].ToString());
                else
                    command.Parameters.AddWithValue("FuzzyBin", DBNull.Value);

                DateTime r;
                if (!String.IsNullOrEmpty(dc.SelectedValues["Repair Date (mm/dd/yyyy)"]) && DateTime.TryParse(dc.SelectedValues["Repair Date (mm/dd/yyyy)"], out r))
                    command.Parameters.AddWithValue("RepairDate", dc.SelectedValues["Repair Date (mm/dd/yyyy)"]);
                else
                    command.Parameters.AddWithValue("RepairDate", DBNull.Value);
                DataSet ds = SQLAccess.GetDataSet(Server, Database, "", "", command);

                #endregion

                if (ds.Tables[0].Rows.Count > 0)
                {

                    /*CountGrp cgrp = new CountGrp(session.ConnectionPool);
                    CountPrt cp = new CountPrt(session.ConnectionPool);

                    bool morePages;
                    if (cgrp.GetRows("Plant = '" + dc.SelectedValues["Plant"] + "' AND CountStatus <> 'c' BY GroupID", 500, 0, out morePages).CountGrp.Rows.Count > 0)
                    {
                        MessageBox.Show("There is an unfinished count group.  Please complete processing of existing count group before creating another", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string id = DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                        id += cgrp.GetRows("Plant = '" + dc.SelectedValues["Plant"] + "' AND GroupID >= '" + id + "' BY GroupID", 500, 0, out morePages).CountGrp.Rows.Count.ToString("00");

                        CountGrpDataSet cds = new CountGrpDataSet();
                        cgrp.GetNewCountGrp(cds, session.PlantID);
                        CountGrpDataSet.CountGrpRow crow = cds.CountGrp.Rows[0] as CountGrpDataSet.CountGrpRow;
                        crow.GroupID = id;
                        crow.Description = "R - " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ", " + dc.SelectedValues["ABC Code"] + ", " + dc.SelectedValues["Limit"];

                        WarehousesAvailableDataSet wds = cgrp.GetCountGrpWarehouses(id, true);
                        foreach (WarehousesAvailableDataSet.WarehousesAvailableRow row in wds.WarehousesAvailable.Rows)
                        {
                            crow.WarehouseList += (crow.WarehouseList.Length > 0 ? "~" : "") + row.WarehouseCode;
                        }

                        cgrp.Update(cds);

                        CountPrtDataSet pds = new CountPrtDataSet();

                        List<string> addedparts = new List<string>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (!addedparts.Contains(row["partnum"].ToString()))
                            {
                                cp.GetNewCountPrt(pds, id);
                                CountPrtDataSet.CountPrtRow prow = pds.CountPrt.Rows[pds.CountPrt.Rows.Count - 1] as CountPrtDataSet.CountPrtRow;
                                prow.PartNum = row["partnum"].ToString();
                                prow.Class = row["classID"].ToString();
                                prow.PartNumPartDescription = row["partdescription"].ToString();
                                prow.PartNumTrackDimension = row["trackdimension"].ToString() == "1";
                                prow.PartNumTrackLots = row["tracklots"].ToString() == "1";
                                prow.PartNumTrackSerialNum = row["trackserialnum"].ToString() == "1";
                                cp.Update(pds);
                                DataRow newRow = dt.NewRow();
                                newRow["Count Group"] = id;
                                newRow["Part"] = row["partnum"].ToString();
                                addedparts.Add(row["partnum"].ToString());

                                dt.Rows.Add(newRow);
                            }
                        }

                        cgrp.Update(cds);
                    }*/
                    throw new Exception("Not Implemented");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.Source + " - " + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        #endregion

        #endregion

        #region Event Handlers

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tab = TabName(((TabControl)sender).SelectedTab);
            favoriteOnButton.Enabled = false;
            favoriteOffButton.Enabled = false;
            ToggleToolStripButtons(tab, false);
            DestroyDateFilter();
            foreach (Module m in Modules.Values)
                foreach (string filter in m.Filters.Keys)
                {
                    toolStrip1.Items.RemoveByKey("lbl_Filter" + filter.Replace(" ", ""));
                    toolStrip1.Items.RemoveByKey("cbl_Filter" + filter.Replace(" ", ""));
                }

            if (tab != "Dashboard")
            {
                if (Modules[tab].Actions.Contains(Module.ModuleAction.Favorite))
                {
                    if (!GetKeyValue("FavoriteTasks").Contains(tab))
                        favoriteOnButton.Enabled = true;
                    else
                        favoriteOffButton.Enabled = true;
                }

                if (Modules[tab].State == Module.ModuleState.Loaded)
                {
                    ToggleToolStripButtons(tab, true);
                    BuildDateFilter(tab);
                    foreach (string filter in Modules[tab].Filters.Keys)
                    {
                        ToolStripLabel lbl = new ToolStripLabel();
                        lbl.Name = "lbl_Filter" + filter.Replace(" ", "");
                        lbl.Text = filter;
                        toolStrip1.Items.Add(lbl);
                        ToolStripComboBox cbl = new ToolStripComboBox();
                        cbl.Name = "cbl_Filter" + filter.Replace(" ", "");
                        cbl.DropDownStyle = ComboBoxStyle.DropDownList;
                        cbl.SelectedIndexChanged += new EventHandler(filterCB_SelectedIndexChanged);
                        foreach (string key in Modules[tab].Filters[filter].Keys)
                            cbl.Items.Add(key);
                        toolStrip1.Items.Add(cbl);
                        if (Modules[tab].SelectedFilter.ContainsKey(filter))
                            ((ToolStripComboBox)toolStrip1.Items["cbl_Filter" + filter.Replace(" ", "")]).SelectedItem = Modules[tab].SelectedFilter[filter];
                    }
                }

                DataGridView dgv = ((TabControl)sender).SelectedTab.Controls.Find("dgv_Data", false)[0] as DataGridView;
                ThreadSafeModify(dgv, delegate { dgv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells); });
            }
        }

        private void ActivateTaskFromControl(object sender, EventArgs e)
        {
            ActivateTask(((Control)sender).Text);
            DrawTaskPage(new string[] { ((Control)sender).Text });
        }

        private void ActivateTaskFromMenu(object sender, EventArgs e)
        {
            if (TabName(tabControl1.SelectedTab) != sender.ToString())
            {
                ActivateTask(sender.ToString());
                DrawTaskPage(new string[] { sender.ToString() });
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ShowConnectionWindow();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConnectionWindow();
        }

        void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            string tab = TabName(tabControl1.SelectedTab);
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    #region Detail Column Actions

                    switch (tab)
                    {
                        case "WBM CS/ENG Usage Report":
                            string message_id = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                            Modules["WBM CS/ENG Usage Report"].Breadcrumb = "Message #" + message_id;
                            Modules["WBM CS/ENG Usage Report"].State = Module.ModuleState.Unloaded;
                            ActivateTask("Message Details");
                            RefreshTab(new string[] { "Message Details", message_id });
                            break;
                        case "Print Part Attach Files For Jobs":
                            {
                                string jobnum = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                                string partnum = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                                Modules["Part Attach File List For Jobs"].Breadcrumb = "Job # " + jobnum;
                                Modules["Part Attach File List For Jobs"].State = Module.ModuleState.Unloaded;
                                ActivateTask("Part Attach File List For Jobs");
                                DrawTaskPage(new string[] { "Part Attach File List For Jobs", jobnum, partnum });
                            }
                            break;
                        case "Print Part Attach Files For MOMs":
                            {
                                string part = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                                Modules["Part Attach File List For MOMs"].Breadcrumb = "MOM Part " + part;
                                Modules["Part Attach File List For MOMs"].State = Module.ModuleState.Unloaded;

                                ActivateTask("Part Attach File List For MOMs");
                                DrawTaskPage(new string[] { "Part Attach File List For MOMs", part });
                            }
                            break;
                        case "Purchase Receipt Summaries For G/L Account":
                            {
                                string glaccount = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                                string vendornum = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                                string vendor = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                                Modules["Purchase Receipts For Vendor"].From = Modules[tab].From;
                                Modules["Purchase Receipts For Vendor"].To = Modules[tab].To;
                                Modules["Purchase Receipts For Vendor"].Breadcrumb = "G/L Account " + glaccount + ", Vendor " + vendor + ", From " + Modules[tab].From.ToString("d") + " To " + Modules[tab].To.ToString("d");

                                ActivateTask("Purchase Receipts For Vendor");
                                DrawTaskPage(new string[] { "Purchase Receipts For Vendor", glaccount, vendornum });
                            }
                            break;
                        case "Purchase Receipt Summaries":
                            {
                                string glaccount = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                                Modules["Purchase Receipt Summaries For G/L Account"].Breadcrumb = glaccount + ", From " + Modules[tab].From.ToString("d") + " To " + Modules[tab].To.ToString("d");
                                Modules["Purchase Receipt Summaries For G/L Account"].From = Modules[tab].From;
                                Modules["Purchase Receipt Summaries For G/L Account"].To = Modules[tab].To;
                                ActivateTask("Purchase Receipt Summaries For G/L Account");
                                RefreshTab(new string[] { "Purchase Receipt Summaries For G/L Account", glaccount });
                            }
                            break;
                        case "Sales Orders":
                            {
                                string ordernum = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                                ActivateTask("Order Lines");
                                RefreshTab(new string[] { "Order Lines", ordernum });
                                ActivateTask("Order Misc Charges");
                                RefreshTab(new string[] { "Order Misc Charges", ordernum });
                                ActivateTask("Order Releases");
                                RefreshTab(new string[] { "Order Releases", ordernum });
                                ActivateTask("Order Memos");
                                RefreshTab(new string[] { "Order Memos", ordernum });
                            }
                            break;
                        case "Get Sales Orders Missing Parts":
                            {
                                string ordernum = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                                string orderline = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                                string orderrelnum = dgv.Rows[e.RowIndex].Cells[3].Value.ToString();
                                Modules["Get Order Parts Below Req Qty"].SelectedFilter["Visibility"] = "Visible";
                                Modules["Get Order Parts Below Req Qty"].Breadcrumb = "Order " + ordernum + ", Line " + orderline + ", Rel " + orderrelnum;
                                ActivateTask("Get Order Parts Below Req Qty");
                                RefreshTab(new string[] { "Get Order Parts Below Req Qty", ordernum, orderline, orderrelnum });
                            }
                            break;
                        case "Get Jobs Missing Parts":
                            {
                                string jobnum = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                                Modules["Get Jobs Missing Parts Details"].SelectedFilter["Part Type"] = "All";
                                Modules["Get Jobs Missing Parts Details"].SelectedFilter["Visibility"] = "Visible";
                                Modules["Get Jobs Missing Parts Details"].Breadcrumb = "Job " + jobnum;
                                ActivateTask("Get Jobs Missing Parts Details");
                                RefreshTab(new string[] { "Get Jobs Missing Parts Details", jobnum });
                            }
                            break;
                        case "Get Order Parts Below Req Qty":
                        case "Get Jobs Missing Parts Details":
                            {
                                string partnum = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                                string revision = dgv.Rows[e.RowIndex].Cells[14].Value.ToString();
                                string warehouse = dgv.Rows[e.RowIndex].Cells[15].Value.ToString();
                                if (tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() == "Get Jobs Missing Parts Details")
                                {
                                    partnum = dgv.Rows[e.RowIndex].Cells[7].Value.ToString();
                                    revision = dgv.Rows[e.RowIndex].Cells[14].Value.ToString();
                                    warehouse = dgv.Rows[e.RowIndex].Cells[15].Value.ToString();
                                }
                                Modules["Get Parts Below Req Qty Details"].Breadcrumb = "Part " + partnum + ", Rev " + revision + ", Warehouse " + warehouse;
                                ActivateTask("Get Parts Below Req Qty Details");
                                RefreshTab(new string[] { "Get Parts Below Req Qty Details", partnum, revision, warehouse });
                            }
                            break;
                        case "Get Job Manuals":
                            {
                                string manualjobnum = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                                ActivateTask("Job Manual Page 1");
                                RefreshTab(new string[] { "Job Manual Page 1", manualjobnum });
                                ActivateTask("Job Manual Page 2");
                                RefreshTab(new string[] { "Job Manual Page 2", manualjobnum });
                                ActivateTask("Job Manual Page 1");
                            }
                            break;
                    }

                    #endregion
                }
                else if ((Modules[tab].AllowDrillDown && Modules[tab].Columns[e.ColumnIndex - 1].ActionColumn) || (!Modules[tab].AllowDrillDown && Modules[tab].Columns[e.ColumnIndex].ActionColumn))
                {
                    #region Action Column Actions

                    switch (tab)
                    {
                        case "Shipment Notifications":
                            {
                                string custid = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                                string ordernum = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                                string packnum = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                                string recipient = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();

                                if (MessageBox.Show("Confirm resend shipment notification for Pack # " + packnum + " to " + recipient, "Confirm?", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                                    ResendShipmentNotification(packnum);
                            }
                            break;
                        case "Get Jobs Missing Parts":
                            string column = "";
                            if (Modules[tab].AllowDrillDown)
                                column = Modules[tab].Columns[e.ColumnIndex - 1].Name;
                            else
                                column = Modules[tab].Columns[e.ColumnIndex].Name;

                            if (column == "Change Visibility")
                            {
                                string p = dgv.Rows[e.RowIndex].Cells[5].Value.ToString();
                                string j = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                                Boolean a = dgv.Rows[e.RowIndex].Cells[1].Value.ToString() == "Hide";
                                ChangeDisplaySettings(j, "", "", "", "", p, a);
                                ((DataTable)dgv.DataSource).Columns["Visibility"].ReadOnly = false;
                                ((DataTable)dgv.DataSource).Columns["Change Visibility"].ReadOnly = false;
                                dgv.Rows[e.RowIndex].Cells["Visibility"].Value = a ? "HIDDEN" : "VISIBLE";
                                dgv.Rows[e.RowIndex].Cells["Change Visibility"].Value = a ? "Show" : "Hide";
                                ((DataTable)dgv.DataSource).Columns["Visibility"].ReadOnly = true;
                                ((DataTable)dgv.DataSource).Columns["Change Visibility"].ReadOnly = true;

                                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgv.DataSource];
                                currencyManager1.SuspendBinding();

                                foreach (DataGridViewRow row in dgv.Rows)
                                    ThreadSafeModify(dgv, delegate { row.Visible = true; });

                                foreach (DataGridViewRow row in dgv.Rows)
                                {
                                    bool visible = true;
                                    foreach (string filter in Modules[tab].Filters.Keys)
                                    {
                                        string filterValue = Modules[tab].Filters[filter][((ToolStripComboBox)toolStrip1.Items["cbl_Filter" + filter.Replace(" ", "")]).SelectedItem.ToString()];
                                        if (filterValue != "%" && row.Cells[filter].Value.ToString() != filterValue)
                                            visible = false;
                                    }
                                    if (!visible)
                                        ThreadSafeModify(dgv, delegate { row.Visible = false; });
                                }
                                currencyManager1.ResumeBinding();
                            }
                            else if (column == "Backflush On")
                            {
                                Cursor c = Cursor;
                                Cursor = Cursors.WaitCursor;
                                string j = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                                ChangeBackFlush(j, true);
                                Cursor = c;
                            }
                            else if (column == "Backflush Off")
                            {
                                Cursor c = Cursor;
                                Cursor = Cursors.WaitCursor;
                                string j = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                                ChangeBackFlush(j, false);
                                Cursor = c;
                            }
                            break;
                        case "Get Order Parts Below Req Qty":
                        case "Get Jobs Missing Parts Details":
                            {
                                string partnum = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                                string jobnum = "";
                                string assembly = "";
                                string operation = "";
                                string ordernum = "";
                                string orderline = "";
                                string orderrelnum = "";
                                Boolean add = false;
                                if (tab == "Get Jobs Missing Parts Details")
                                {
                                    partnum = dgv.Rows[e.RowIndex].Cells[7].Value.ToString();
                                    jobnum = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                                    assembly = dgv.Rows[e.RowIndex].Cells[3].Value.ToString();
                                    operation = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                                    add = dgv.Rows[e.RowIndex].Cells[1].Value.ToString() == "Hide";
                                }
                                else
                                {
                                    string o = tabControl1.SelectedTab.Text.Replace("Get Order Parts Below Req Qty -> Order ", "");
                                    o = o.Replace("Line ", "");
                                    o = o.Replace("Rel ", "");
                                    ordernum = o.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0];
                                    orderline = o.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[1];
                                    orderrelnum = o.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[2];
                                    add = dgv.Rows[e.RowIndex].Cells[1].Value.ToString() == "Hide";
                                }
                                ChangeDisplaySettings(jobnum, assembly, operation, ordernum, orderline, partnum, add);
                                if (tab == "Get Jobs Missing Parts Details" && dc.SelectedValues["Issue/Return?"] == "Yes" && dgv.Rows[e.RowIndex].Cells[27].Value.ToString() == "1" && dgv.Rows[e.RowIndex].Cells[28].Value.ToString() == "0")
                                {
                                    Cursor c = Cursor;
                                    Cursor = Cursors.WaitCursor;
                                    int mtlseq = Int32.Parse(dgv.Rows[e.RowIndex].Cells[5].Value.ToString());
                                    if (add)
                                    {
                                        decimal qty = Decimal.Parse(dgv.Rows[e.RowIndex].Cells[8].Value.ToString());
                                        if (dgv.Rows[e.RowIndex].Cells[25].Value.ToString() == "1")
                                        {
                                            ChangeBackFlush(jobnum, Int32.Parse(assembly), mtlseq, false);
                                            ((DataTable)dgv.DataSource).Columns["Backflush"].ReadOnly = false;
                                            dgv.Rows[e.RowIndex].Cells["Backflush"].Value = "0";
                                            dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                                            ((DataTable)dgv.DataSource).Columns["Backflush"].ReadOnly = true;
                                        }
                                        if (qty > 0)
                                        {
                                            IssueMaterial(session.CompanyID, jobnum, Int32.Parse(assembly), mtlseq, qty);
                                            ((DataTable)dgv.DataSource).Columns["Issued Qty"].ReadOnly = false;
                                            dgv.Rows[e.RowIndex].Cells["Issued Qty"].Value = qty;
                                            ((DataTable)dgv.DataSource).Columns["Issued Qty"].ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        decimal qty = Decimal.Parse(dgv.Rows[e.RowIndex].Cells[29].Value.ToString());
                                        if (String.IsNullOrEmpty(dgv.Rows[e.RowIndex].Cells[26].Value.ToString()))
                                        {
                                            ChangeBackFlush(jobnum, Int32.Parse(assembly), mtlseq, true);
                                            ((DataTable)dgv.DataSource).Columns["Backflush"].ReadOnly = false;
                                            dgv.Rows[e.RowIndex].Cells["Backflush"].Value = "1";
                                            dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                                            ((DataTable)dgv.DataSource).Columns["Backflush"].ReadOnly = true;
                                        }
                                        else
                                        {
                                            if (dgv.Rows[e.RowIndex].Cells[25].Value.ToString() != dgv.Rows[e.RowIndex].Cells[26].Value.ToString())
                                            {
                                                ChangeBackFlush(jobnum, Int32.Parse(assembly), mtlseq, dgv.Rows[e.RowIndex].Cells[26].Value.ToString() == "1");
                                                ((DataTable)dgv.DataSource).Columns["Backflush"].ReadOnly = false;
                                                dgv.Rows[e.RowIndex].Cells["Backflush"].Value = dgv.Rows[e.RowIndex].Cells[26].Value.ToString();
                                                dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = dgv.Rows[e.RowIndex].Cells[26].Value.ToString() == "1" ? Color.LightBlue : Color.White;
                                                ((DataTable)dgv.DataSource).Columns["Backflush"].ReadOnly = true;
                                            }
                                        }
                                        if (qty > 0)
                                        {
                                            ReturnMaterial(session.CompanyID, jobnum, Int32.Parse(assembly), mtlseq, qty);
                                            ((DataTable)dgv.DataSource).Columns["Issued Qty"].ReadOnly = false;
                                            dgv.Rows[e.RowIndex].Cells["Issued Qty"].Value = 0;
                                            ((DataTable)dgv.DataSource).Columns["Issued Qty"].ReadOnly = true;
                                        }
                                    }
                                    Cursor = c;
                                }
                                ((DataTable)dgv.DataSource).Columns["Visibility"].ReadOnly = false;
                                ((DataTable)dgv.DataSource).Columns["Change Visibility"].ReadOnly = false;
                                dgv.Rows[e.RowIndex].Cells["Visibility"].Value = add ? "HIDDEN" : "VISIBLE";
                                dgv.Rows[e.RowIndex].Cells["Change Visibility"].Value = add ? "Show" : "Hide";
                                ((DataTable)dgv.DataSource).Columns["Visibility"].ReadOnly = true;
                                ((DataTable)dgv.DataSource).Columns["Change Visibility"].ReadOnly = true;

                                CurrencyManager cManager1 = (CurrencyManager)BindingContext[dgv.DataSource];
                                cManager1.SuspendBinding();

                                foreach (DataGridViewRow row in dgv.Rows)
                                    ThreadSafeModify(dgv, delegate { row.Visible = true; });

                                foreach (DataGridViewRow row in dgv.Rows)
                                {
                                    bool visible = true;
                                    foreach (string filter in Modules[tab].Filters.Keys)
                                    {
                                        string filterValue = Modules[tab].Filters[filter][((ToolStripComboBox)toolStrip1.Items["cbl_Filter" + filter.Replace(" ", "")]).SelectedItem.ToString()];
                                        if (filterValue != "%" && row.Cells[filter].Value.ToString() != filterValue)
                                            visible = false;
                                    }
                                    if (!visible)
                                        ThreadSafeModify(dgv, delegate { row.Visible = false; });
                                }
                                cManager1.ResumeBinding();
                            }
                            break;
                    }

                    #endregion
                }
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dgv = tabControl1.SelectedTab.Controls.Find("dgv_Data", false)[0] as DataGridView;
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DataObject obj = dgv.GetClipboardContent();
            Clipboard.SetDataObject(obj, true);
        }

        void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            string task = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            foreach (GridColumn column in Modules[task].Columns)
            {
                if (column.UpdateTriggerColumns.Contains(dgv.Columns[e.ColumnIndex].Name))
                {
                    object value1 = dgv.Rows[e.RowIndex].Cells[column.Function.LeftColumn].Value;
                    object value2 = dgv.Rows[e.RowIndex].Cells[column.Function.RightColumn].Value;
                    ((DataTable)dgv.DataSource).Columns[column.Name].ReadOnly = false;
                    dgv.Columns[column.Name].ReadOnly = false;
                    dgv.Rows[e.RowIndex].Cells[column.Name].Value = Decimal.Parse(value1.ToString()) * Decimal.Parse(value2.ToString());
                    dgv.Columns[column.Name].ReadOnly = column.ReadOnly;
                    ((DataTable)dgv.DataSource).Columns[column.Name].ReadOnly = column.ReadOnly;
                }
                if (column.EditTextAction)
                {
                    switch (task)
                    {
                        case "Get Jobs Missing Parts Details":
                            string comment = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                            string partnum = dgv.Rows[e.RowIndex].Cells[7].Value.ToString();
                            string jobnum = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                            string assembly = dgv.Rows[e.RowIndex].Cells[3].Value.ToString();
                            string operation = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                            EnterMissingPartComment(jobnum, assembly, operation, partnum, comment);
                            break;
                    }
                }
            }
        }

        void dgv_Sorted(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            string tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgv.DataSource];
            currencyManager1.SuspendBinding();

            foreach (DataGridViewRow row in dgv.Rows)
                ThreadSafeModify(dgv, delegate { row.Visible = true; });

            foreach (DataGridViewRow row in dgv.Rows)
            {
                bool visible = true;
                foreach (string filter in Modules[tab].Filters.Keys)
                {
                    string filterValue = Modules[tab].Filters[filter][((ToolStripComboBox)toolStrip1.Items["cbl_Filter" + filter.Replace(" ", "")]).SelectedItem.ToString()];
                    if (filterValue != "%" && row.Cells[filter].Value.ToString() != filterValue)
                        visible = false;
                }
                if (!visible)
                    ThreadSafeModify(dgv, delegate { row.Visible = false; });
            }
            currencyManager1.ResumeBinding();
        }

        void backWholeMonth_Click(object sender, EventArgs e)
        {
            DateTimePicker from = toolStrip1.Controls.Find("fromMonth", true)[0] as DateTimePicker;
            DateTimePicker to = toolStrip1.Controls.Find("toMonth", true)[0] as DateTimePicker;

            DateTime originalFrom = DateTime.Parse(from.Text);
            DateTime newTo = new DateTime(originalFrom.Year, originalFrom.Month, 1).AddDays(-1);
            to.Text = newTo.ToShortDateString();
            from.Text = new DateTime(newTo.Year, newTo.Month, 1).ToShortDateString();
        }

        void nextWholeMonth_Click(object sender, EventArgs e)
        {
            DateTimePicker from = toolStrip1.Controls.Find("fromMonth", true)[0] as DateTimePicker;
            DateTimePicker to = toolStrip1.Controls.Find("toMonth", true)[0] as DateTimePicker;

            DateTime originalTo = DateTime.Parse(to.Text);
            DateTime newFrom = new DateTime(originalTo.Year, originalTo.Month, 1).AddMonths(1);
            from.Text = newFrom.ToShortDateString();
            to.Text = newFrom.AddMonths(1).AddDays(-1).ToShortDateString();
        }

        void nextMonth_Click(object sender, EventArgs e)
        {
            DateTimePicker from = toolStrip1.Controls.Find("fromMonth", true)[0] as DateTimePicker;
            DateTimePicker to = toolStrip1.Controls.Find("toMonth", true)[0] as DateTimePicker;

            from.Text = DateTime.Parse(to.Text).AddDays(1).ToShortDateString();
            to.Text = DateTime.Parse(to.Text).AddMonths(1).ToShortDateString();
        }

        void backMonth_Click(object sender, EventArgs e)
        {
            DateTimePicker from = toolStrip1.Controls.Find("fromMonth", true)[0] as DateTimePicker;
            DateTimePicker to = toolStrip1.Controls.Find("toMonth", true)[0] as DateTimePicker;

            to.Text = DateTime.Parse(from.Text).AddDays(-1).ToShortDateString();
            from.Text = DateTime.Parse(from.Text).AddMonths(-1).ToShortDateString();
        }

        void fromMonth_TextChanged(object sender, EventArgs e)
        {
            string tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            Modules[tab].From = DateTime.Parse(((DateTimePicker)sender).Text);
        }

        void toMonth_TextChanged(object sender, EventArgs e)
        {
            string tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            Modules[tab].To = DateTime.Parse(((DateTimePicker)sender).Text);
        }

        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                webBrowser1.ShowPrintDialog();
            }
        }

        #region Tool Strip Buttons

        private void favoriteOnButton_Click(object sender, EventArgs e)
        {
            string value = GetKeyValue("FavoriteTasks");
            if (!String.IsNullOrEmpty(value))
                value += ",";
            SetKeyValue("FavoriteTasks", value + tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim());
            favoriteOnButton.Enabled = false;
            favoriteOffButton.Enabled = true;
            DrawDashboard();
        }

        private void favoriteOffButton_Click(object sender, EventArgs e)
        {
            string value = GetKeyValue("FavoriteTasks");
            string[] split = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string newValue = "";
            foreach (String str in split)
                if (str != tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim())
                {
                    if (!String.IsNullOrEmpty(newValue))
                        newValue += ",";
                    newValue += str;
                }
            SetKeyValue("FavoriteTasks", newValue);
            favoriteOnButton.Enabled = true;
            favoriteOffButton.Enabled = false;
            DrawDashboard();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            RefreshTab(new string[] { tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() });
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            bool action = false;
            switch (tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim())
            {
                case "Batch Material Replacement":
                    {
                        DataGridView dgv = tabControl1.SelectedTab.Controls.Find("dgv_Data", false)[0] as DataGridView;
                        dgv.EndEdit();
                        List<DataGridViewRow> rowsToSave = new List<DataGridViewRow>();
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.Cells["Confirm"].Value.ToString() == "Yes")
                                rowsToSave.Add(row);
                        }

                        if (rowsToSave.Count == 0)
                            MessageBox.Show("You have not confirmed any rows to process", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        else
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(DoJobMaterialReplacement), rowsToSave);
                            action = true;
                        }
                    }
                    break;
                case "Power Analyzer Data":
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SavePowerAnalyzerData));
                    action = true;
                    break;
                case "Sales Rep Mgmt":
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SaveSalesRepMgmt));
                    action = true;
                    break;
                case "Edit Job/Order Dates":
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SaveJobsMismatchedOrderDates));
                    action = true;
                    break;
                case "Part Min Qty Alerts":
                    {
                        DataGridView dgv = tabControl1.SelectedTab.Controls.Find("dgv_Data", false)[0] as DataGridView;
                        List<DataGridViewRow> rowsToSave = new List<DataGridViewRow>();
                        foreach (DataGridViewRow row in dgv.Rows)
                            if (row.Cells["Process"].Value.ToString() == "Yes")
                                rowsToSave.Add(row);

                        if (rowsToSave.Count == 0)
                            MessageBox.Show("You have not selected any rows to process", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        else
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(SavePartMinQty), rowsToSave);
                            action = true;
                        }
                    }
                    break;
                case "Outsourced Material Management":
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SaveOutsourcedMaterialManagement));
                    action = true;
                    break;
            }
            if (action)
            {
                refreshButton.Enabled = false;
                saveButton.Enabled = false;
                printButton.Enabled = false;
                closeButton.Enabled = false;
                lock (Modules[tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim()])
                {
                    Modules[tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim()].State = Module.ModuleState.Saving;
                }

                DrawTaskPage(new string[] { tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() });
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            DataGridView dgv = tabControl1.SelectedTab.Controls.Find("dgv_Data", false)[0] as DataGridView;

            string module = TabName(tabControl1.SelectedTab);

            foreach (DataGridViewCell cell in dgv.SelectedCells)
            {
                dgv.Rows[cell.RowIndex].Selected = true;
            }

            switch (module)
            {
                case "Get Jobs Missing Parts Details":
                    foreach (DataGridViewRow row in dgv.Rows)
                        row.Selected = false;
                    foreach (DataGridViewRow row in dgv.Rows)
                        if (row.Visible)
                            row.Selected = true;
                    PrintJobMissingPartsList(new object[] { "Get Jobs Missing Parts Details", "Missing Parts List For " + TabDetails(tabControl1.SelectedTab), dgv.SelectedRows, ((ToolStripComboBox)toolStrip1.Items["cbl_FilterMtlPartType"]).SelectedItem.ToString(), ((ToolStripComboBox)toolStrip1.Items["cbl_FilterVisibility"]).SelectedItem.ToString() });
                    foreach (DataGridViewRow row in dgv.Rows)
                        if (row.Visible)
                            row.Selected = false;
                    break;
                case "Part Attach File List For Jobs":
                    if (dgv.SelectedRows.Count == 0)
                        MessageBox.Show("You have not selected any rows to print", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    {
                        refreshButton.Enabled = false;
                        saveButton.Enabled = false;
                        printButton.Enabled = false;
                        closeButton.Enabled = false;
                        lock (Modules[module])
                        {
                            Modules[module].State = Module.ModuleState.Printing;
                            DrawTaskPage(new string[] { module });
                        }
                        PrintPartAttachments(new object[] { "Part Attach File List For Jobs", dgv.SelectedRows });
                    }
                    break;
                case "Part Attach File List For MOMs":
                    if (dgv.SelectedRows.Count == 0)
                        MessageBox.Show("You have not selected any rows to print", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    {
                        refreshButton.Enabled = false;
                        saveButton.Enabled = false;
                        printButton.Enabled = false;
                        closeButton.Enabled = false;
                        lock (Modules[module])
                        {
                            Modules[module].State = Module.ModuleState.Printing;
                            DrawTaskPage(new string[] { module });
                        }
                        PrintPartAttachments(new object[] { "Part Attach File List For MOMs", dgv.SelectedRows });
                    }
                    break;
                case "Customer Info":
                    if (dgv.SelectedRows.Count == 0)
                        MessageBox.Show("You have not selected any rows to print", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    {
                        refreshButton.Enabled = false;
                        saveButton.Enabled = false;
                        printButton.Enabled = false;
                        closeButton.Enabled = false;
                        lock (Modules[module])
                        {
                            Modules[module].State = Module.ModuleState.Printing;
                            DrawTaskPage(new string[] { module });
                        }
                        PrintCustomerInfo(new object[] { "Customer Info", dgv.SelectedRows });
                    }
                    break;
                case "Missing Part Checklist":
                    foreach (DataGridViewRow row in dgv.Rows)
                        if (row.Visible)
                            row.Selected = true;
                    PrintMissingPartChecklist(new object[] { "Missing Part Checklist", dgv.SelectedRows });
                    foreach (DataGridViewRow row in dgv.Rows)
                        if (row.Visible)
                            row.Selected = false;
                    break;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            string tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            Modules[tab].State = Module.ModuleState.Unloaded;
            refreshButton.Enabled = false;
            saveButton.Enabled = false;
            printButton.Enabled = false;
            closeButton.Enabled = false;
            DestroyDateFilter();

            int index = tabControl1.SelectedIndex;
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            tabControl1.SelectTab(index - 1);
            if (index > 1)
            {
                tab = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                ToggleToolStripButtons(tab, true);
            }
        }

        private void filterCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridView dgv = new DataGridView();
            ThreadSafeModify(tabControl1, delegate { dgv = tabControl1.SelectedTab.Controls.Find("dgv_Data", false)[0] as DataGridView; });
            if (dgv.DataSource != null)
            {
                string module = "";
                ThreadSafeModify(tabControl1, delegate { module = tabControl1.SelectedTab.Text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim(); });
                Modules[module].SelectedFilter[((ToolStripComboBox)sender).Name.Replace("cbl_Filter", "")] = ((ToolStripComboBox)sender).SelectedItem.ToString();

                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgv.DataSource];
                currencyManager1.SuspendBinding();

                foreach (DataGridViewRow row in dgv.Rows)
                    ThreadSafeModify(dgv, delegate { row.Visible = true; });

                /*                foreach (DataGridViewRow row in dgv.Rows)
                                {
                                    bool visible = true;
                                    foreach (string filter in Modules[module].Filters.Keys)
                                    {
                                        object selected = null;
                                        if (toolStrip1.Items["cbl_Filter" + filter.Replace(" ", "")] != null)
                                            ThreadSafeModify(toolStrip1, delegate { selected = ((ToolStripComboBox)toolStrip1.Items["cbl_Filter" + filter.Replace(" ", "")]).SelectedItem; });

                                        if (selected != null)
                                        {
                                            string filterValue = Modules[module].Filters[filter][selected.ToString()];
                                            if (filterValue != "%" && row.Cells[filter].Value.ToString() != filterValue)
                                                visible = false;
                                        }
                                    }
                                    if (!visible)
                                        ThreadSafeModify(dgv, delegate { row.Visible = false; });
                                }
                */
                foreach (string filter in Modules[module].Filters.Keys)
                {
                    object selected = null;
                    if (toolStrip1.Items["cbl_Filter" + filter.Replace(" ", "")] != null)
                        ThreadSafeModify(toolStrip1, delegate { selected = ((ToolStripComboBox)toolStrip1.Items["cbl_Filter" + filter.Replace(" ", "")]).SelectedItem; });

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        bool visible = true;

                        if (selected != null)
                        {
                            string filterValue = Modules[module].Filters[filter][selected.ToString()];
                            if (filterValue != "%" && row.Cells[filter].Value.ToString() != filterValue)
                                visible = false;
                        }
                        if (!visible)
                            ThreadSafeModify(dgv, delegate { row.Visible = false; });
                    }
                }
                currencyManager1.ResumeBinding();
            }
        }

        #endregion

        #endregion
    }

    #region Class DataFunction

    internal class DataFunction
    {
        public string LeftColumn { get; set; }
        public string RightColumn { get; set; }

        public DataFunction(string left, string right)
        {
            LeftColumn = left;
            RightColumn = right;
        }
    }

    #endregion

    #region Class GridColumn

    internal class GridColumn
    {
        public string Name { get; set; }
        public bool ReadOnly { get; set; }
        public Type DataType { get; set; }
        public string CellFormat { get; set; }
        public List<string> UpdateTriggerColumns { get; set; }
        public DataFunction Function { get; set; }
        public bool Frozen { get; set; }
        public bool Visible { get; set; }
        public List<string> ValidValues { get; set; }
        public bool ActionColumn { get; set; }
        public bool EditTextAction { get; set; }
        public bool Image { get; set; }

        public GridColumn(string name)
        {
            Name = name;
            ReadOnly = true;
            DataType = typeof(String);
            CellFormat = "";
            UpdateTriggerColumns = new List<string>();
            Frozen = false;
            Visible = true;
            ValidValues = new List<string>();
            ActionColumn = false;
            EditTextAction = false;
        }

        public GridColumn(string name, Type t)
        {
            Name = name;
            ReadOnly = true;
            if (t == typeof(Image))
                Image = true;
            DataType = t;
            CellFormat = "";
            UpdateTriggerColumns = new List<string>();
            Frozen = false;
            Visible = true;
            ValidValues = new List<string>();
            ActionColumn = false;
            EditTextAction = false;
        }

        public GridColumn(string name, bool ro)
        {
            Name = name;
            ReadOnly = ro;
            DataType = typeof(String);
            CellFormat = "";
            UpdateTriggerColumns = new List<string>();
            Frozen = false;
            Visible = true;
            ValidValues = new List<string>();
            ActionColumn = false;
            EditTextAction = false;
        }

        public GridColumn(string name, bool ro, bool frozen)
        {
            Name = name;
            ReadOnly = ro;
            DataType = typeof(String);
            CellFormat = "";
            UpdateTriggerColumns = new List<string>();
            Frozen = frozen;
            Visible = true;
            ValidValues = new List<string>();
            ActionColumn = false;
            EditTextAction = false;
        }


    }

    #endregion

    #region Class Module

    internal class Module
    {
        #region Enums

        public enum ModuleState { Unloaded, Loading, Loaded, Saving, Printing, Saved }
        public enum ModuleAction { Save, Print, Favorite, Refresh }

        #endregion

        #region Values

        public ModuleState State { get; set; }
        public DateTime LastUpdate { get; set; }
        public System.Data.DataTable Data { get; set; }
        public List<ModuleAction> Actions { get; set; }
        public bool AllowDrillDown { get; set; }
        public string Breadcrumb { get; set; }
        public Dictionary<string, Dictionary<string, string>> Filters { get; set; }
        public Dictionary<string, string> SelectedFilter { get; set; }
        public List<GridColumn> Columns { get; set; }
        public Dictionary<string, Dictionary<string, string>> PreLoadDataCaptures { get; set; }
        public Dictionary<string, string> DataCaptureSelections { get; set; }
        public bool DateFilter { get; set; }
        public bool DateFilterVisible { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool HasTotalRow { get; set; }
        public DataTable TotalData { get; set; }

        #endregion

        #region Constructors

        public Module(List<ModuleAction> validActions)
        {
            State = ModuleState.Unloaded;
            LastUpdate = DateTime.Now;
            AllowDrillDown = false;
            Breadcrumb = "";
            Filters = new Dictionary<string, Dictionary<string, string>>();
            SelectedFilter = new Dictionary<string, string>();
            Columns = new List<GridColumn>();
            PreLoadDataCaptures = new Dictionary<string, Dictionary<string, string>>();
            DataCaptureSelections = new Dictionary<string, string>();
            DateFilter = false;
            From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            To = DateTime.Now;

            Actions = validActions;
        }

        public Module(List<GridColumn> columns)
        {
            State = ModuleState.Unloaded;
            LastUpdate = DateTime.Now;
            AllowDrillDown = false;
            Breadcrumb = "";
            Filters = new Dictionary<string, Dictionary<string, string>>();
            SelectedFilter = new Dictionary<string, string>();
            Columns = columns;
            PreLoadDataCaptures = new Dictionary<string, Dictionary<string, string>>();
            DataCaptureSelections = new Dictionary<string, string>();
            DateFilter = false;
            From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            To = DateTime.Now;

            Actions = new List<ModuleAction>();
        }

        public Module(List<ModuleAction> validActions, List<GridColumn> columns)
        {
            State = ModuleState.Unloaded;
            LastUpdate = DateTime.Now;
            AllowDrillDown = false;
            Breadcrumb = "";
            Filters = new Dictionary<string, Dictionary<string, string>>();
            SelectedFilter = new Dictionary<string, string>();
            Columns = columns;
            PreLoadDataCaptures = new Dictionary<string, Dictionary<string, string>>();
            DataCaptureSelections = new Dictionary<string, string>();
            DateFilter = false;
            From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            To = DateTime.Now;

            Actions = validActions;
        }

        public bool HasActionColumns()
        {
            bool r = false;
            foreach (GridColumn c in Columns)
            {
                if (c.ActionColumn)
                    r = true;
            }
            return r;
        }

        #endregion
    }

    #endregion

    #region Class SQLAccess
    // TODO:5 Add unit tests
    public static class SQLAccess
    {
        public static DataSet GetDataSet(string server, string database, string username, string password, SqlCommand command)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; Integrated Security=SSPI;", server, database, username, password)))
                {
                    command.Connection = sqlConnection;
                    command.CommandTimeout = 50000;

                    sqlConnection.Open();

                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    sqlConnection.Close();

                    return ds;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static string GetScalar(string server, string database, string username, string password, SqlCommand command)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; Integrated Security=SSPI;", server, database, username, password)))
                {
                    command.Connection = sqlConnection;

                    sqlConnection.Open();

                    string result = command.ExecuteScalar().ToString();

                    sqlConnection.Close();

                    return result;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static string GetScalar(string server, string database, string username, string password, SqlCommand command, SqlConnection connection)
        {
            try
            {
                command.Connection = connection;
                string result = command.ExecuteScalar().ToString();
                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static void NonQuery(string server, string database, string username, string password, SqlCommand command)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; Integrated Security=SSPI;", server, database, username, password)))
                {
                    command.Connection = sqlConnection;

                    sqlConnection.Open();

                    command.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static SqlConnection BeginTransaction(string server, string database, string username, string password)
        {
            SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source = {0}; Initial Catalog = {1}; Integrated Security=SSPI;", server, database, username, password));

            try
            {
                SqlCommand command = new SqlCommand("begin transaction");
                command.Connection = sqlConnection;
                sqlConnection.Open();

                command.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                if (sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();
                throw ex;
            }

            return sqlConnection;
        }

        public static void CommitTransaction(SqlConnection connection)
        {
            try
            {
                SqlCommand command = new SqlCommand("commit transaction");
                command.Connection = connection;
                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public static void RollbackTransaction(SqlConnection connection)
        {
            try
            {
                SqlCommand command = new SqlCommand("rollback transaction");
                command.Connection = connection;
                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }

    #endregion
}
