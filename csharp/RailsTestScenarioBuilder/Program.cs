using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using ObjectLibrary;

namespace RailsTestScenarioBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            MySqlCommand lucommand = new MySqlCommand("SELECT id, quote_line_id, type_id, size_id, construction_id, finish_id, handle_id, light_id, shelf_kit_id, shelf_color_id, post_id, shelf_type_id, number_of_doors, single_endlight, led_id FROM line_ups where quote_line_id is not null");
            DataSet luds = MySQLAccess.GetDataSet("172.16.6.60", "wbq_production", "ror", "SomePW23", lucommand);
            List<string> lines = new List<string>();

            string header = "@price" + Environment.NewLine;
            header += "Feature: a model should have correct pricing" + Environment.NewLine + Environment.NewLine;
            header += "When any user creates a new line-up and saves the line-up, the pricing should be correct" + Environment.NewLine;

            string scenario = "When I sign in as a full user" + Environment.NewLine;
            scenario += "And I add a new blank quote" + Environment.NewLine;
            scenario += "And I create a new lineup" + Environment.NewLine;
            scenario += "And I configure lineup with IDs \"<type>\" and <numdoors> Doors and \"<size>\" and \"<const>\" and \"<finish>\" and \"<handle>\" and \"<light>\" and \"<skit>\" and \"<scolor>\" and \"<spost>\" and \"<stype>\"" + Environment.NewLine;
            scenario += "And I edit the quoteline" + Environment.NewLine;
            scenario += "And I configure locks and hinges with \"<h1>\" and \"<l1>\" and \"<h2>\" and \"<l2>\" and \"<h3>\" and \"<l3>\" and \"<h4>\" and \"<l4>\" and \"<h5>\" and \"<l5>\" and \"<h6>\" and \"<l6>\" and \"<h7>\" and \"<l7>\" and \"<h8>\" and \"<l8>\" and \"<h9>\" and \"<l9>\" and \"<h10>\" and \"<l10>\" and \"<h11>\" and \"<l11>\" and \"<h12>\" and \"<l12>\" and \"<h13>\" and \"<l13>\" and \"<h14>\" and \"<l14>\" and \"<h15>\" and \"<l15>\" and \"<h16>\" and \"<l16>\" and \"<h17>\" and \"<l17>\" and \"<h18>\" and \"<l18>\" and \"<h19>\" and \"<l19>\" and \"<h20>\" and \"<l20>\"" + Environment.NewLine;
            scenario += "And I edit the quoteline" + Environment.NewLine;
            scenario += "And I edit the shelving" + Environment.NewLine;
            scenario += "And I configure shelving with \"<f1stype>\" and \"<f1scolor>\" and \"<f1spost>\" and \"<f1sqty>\" and \"<f1pqty>\" and \"<f1ldqty>\" and \"<f1pgqty>\" and \"<f1gsqty>\" and \"<f1ptmqty>\" and \"<f1bqty>\" and \"<f1ebqty>\" and \"<f2stype>\" and \"<f2scolor>\" and \"<f2spost>\" and \"<f2sqty>\" and \"<f2pqty>\" and \"<f2ldqty>\" and \"<f2pgqty>\" and \"<f2gsqty>\" and \"<f2ptmqty>\" and \"<f2bqty>\" and \"<f2ebqty>\" and \"<f3stype>\" and \"<f3scolor>\" and \"<f3spost>\" and \"<f3sqty>\" and \"<f3pqty>\" and \"<f3ldqty>\" and \"<f3pgqty>\" and \"<f3gsqty>\" and \"<f3ptmqty>\" and \"<f3bqty>\" and \"<f3ebqty>\" and \"<f4stype>\" and \"<f4scolor>\" and \"<f4spost>\" and \"<f4sqty>\" and \"<f4pqty>\" and \"<f4ldqty>\" and \"<f4pgqty>\" and \"<f4gsqty>\" and \"<f4ptmqty>\" and \"<f4bqty>\" and \"<f4ebqty>\"" + Environment.NewLine;
            scenario += "And I configure pricing with \"<discount>\" and \"<adjamt>\" and \"<adjcode>\" and \"<adjamts>\" and \"<adjcodes>\"" + Environment.NewLine;
            scenario += "And I edit the quoteline" + Environment.NewLine;
            scenario += "And I configure misc lineup options with \"<inlay>\" and \"<pushbar>\" and \"<silkscreen>\" and \"<frontkick>\" and \"<backkick>\" and \"<fbqty>\" and \"<bbqty>\" and \"<singleel>\" and \"<led>\"" + Environment.NewLine;
            scenario += "And I retrieve verification data for lineup with \"<type>\" and <numdoors> Doors and \"<size>\" and \"<const>\" and \"<finish>\" and \"<handle>\" and \"<light>\" and \"<skit>\" and \"<scolor>\" and \"<spost>\" and \"<stype>\", with locks and hinges ";
            scenario += "\"<h1>\" and \"<l1>\" and \"<h2>\" and \"<l2>\" and \"<h3>\" and \"<l3>\" and \"<h4>\" and \"<l4>\" and \"<h5>\" and \"<l5>\" and \"<h6>\" and \"<l6>\" and \"<h7>\" and \"<l7>\" and \"<h8>\" and \"<l8>\" and \"<h9>\" and \"<l9>\" and \"<h10>\" and \"<l10>\" and \"<h11>\" and \"<l11>\" and \"<h12>\" and \"<l12>\" and \"<h13>\" and \"<l13>\" and \"<h14>\" and \"<l14>\" and \"<h15>\" and \"<l15>\" and \"<h16>\" and \"<l16>\" and \"<h17>\" and \"<l17>\" and \"<h18>\" and \"<l18>\" and \"<h19>\" and \"<l19>\" and \"<h20>\" and \"<l20>\", with shelving ";
            scenario += "\"<f1stype>\" and \"<f1scolor>\" and \"<f1spost>\" and \"<f1sqty>\" and \"<f1pqty>\" and \"<f1ldqty>\" and \"<f1pgqty>\" and \"<f1gsqty>\" and \"<f1ptmqty>\" and \"<f1bqty>\" and \"<f1ebqty>\" and \"<f2stype>\" and \"<f2scolor>\" and \"<f2spost>\" and \"<f2sqty>\" and \"<f2pqty>\" and \"<f2ldqty>\" and \"<f2pgqty>\" and \"<f2gsqty>\" and \"<f2ptmqty>\" and \"<f2bqty>\" and \"<f2ebqty>\" and \"<f3stype>\" and \"<f3scolor>\" and \"<f3spost>\" and \"<f3sqty>\" and \"<f3pqty>\" and \"<f3ldqty>\" and \"<f3pgqty>\" and \"<f3gsqty>\" and \"<f3ptmqty>\" and \"<f3bqty>\" and \"<f3ebqty>\" and \"<f4stype>\" and \"<f4scolor>\" and \"<f4spost>\" and \"<f4sqty>\" and \"<f4pqty>\" and \"<f4ldqty>\" and \"<f4pgqty>\" and \"<f4gsqty>\" and \"<f4ptmqty>\" and \"<f4bqty>\" and \"<f4ebqty>\" with misc lineup options ";
            scenario += "\"<discount>\" and \"<adjamt>\" and \"<adjcode>\" and \"<adjamts>\" and \"<adjcodes>\" and \"<inlay>\" and \"<pushbar>\" and \"<silkscreen>\" and \"<frontkick>\" and \"<backkick>\" and \"<fbqty>\" and \"<bbqty>\" and \"<singleel>\" and \"<led>\"" + Environment.NewLine;
            scenario += "Then the object price should validate against the service";

/*            string scenario = "| type | numdoors | size | const | finish | handle | light | skit | scolor | spost | stype | h1 | l1 | h2 | l2 | h3 | l3 | h4 | l4 | h5 | l5 | h6 | l6 | h7 | l7 | h8 | l8 | h9 | l9 | h10 | l10 | h11 | l11 | h12 | l12 " +
                "| h13 | l13 | h14 | l14 | h15 | l15 | h16 | l16 | h17 | l17 | h18 | l18 | h19 | l19 | h20 | l20 | f1stype | f1scolor | f1spost | f1sqty | f1pqty | f1ldqty | f1pgqty | f1gsqty | f1ptmqty | f1bqty | f1ebqty | f2stype | f2scolor " +
                "| f2spost | f2sqty | f2pqty | f2ldqty | f2pgqty | f2gsqty | f2ptmqty | f2bqty | f2ebqty | f3stype | f3scolor | f3spost | f3sqty | f3pqty | f3ldqty | f3pgqty | f3gsqty | f3ptmqty | f3bqty | f3ebqty | f4stype | f4scolor | f4spost " +
                "| f4sqty | f4pqty | f4ldqty | f4pgqty | f4gsqty | f4ptmqty | f4bqty | f4ebqty | discount | adjamt | adjcode | adjamts | adjcodes | inlay | pushbar | silkscreen | frontkick | backkick | fbqty | bbqty | fbgloc | bbgloc | singleel | led |";
*/            foreach (DataRow lurow in luds.Tables[0].Rows)
            {
                  string rowentry = scenario;
/*                string rowentry =
                    "| <type> | <numdoors> | <size> | <const> | <finish> | <handle> | <light> | <skit> | <scolor> | <spost> | <stype> "+
                    "| <h1> | <l1> | <h2> | <l2> | <h3> | <l3> | <h4> | <l4> | <h5> | <l5> | <h6> | <l6> | <h7> | <l7> | <h8> | <l8> | <h9> | <l9> "+
                    "| <h10> | <l10> | <h11> | <l11> | <h12> | <l12> | <h13> | <l13> | <h14> | <l14> | <h15> | <l15> | <h16> | <l16> | <h17> | <l17> | <h18> | <l18> | <h19> | <l19> | <h20> | <l20> "+
                    "| <f1stype> | <f1scolor> | <f1spost> | <f1sqty> | <f1pqty> | <f1ldqty> | <f1pgqty> | <f1gsqty> | <f1ptmqty> | <f1bqty> | <f1ebqty> "+
                    "| <f2stype> | <f2scolor> | <f2spost> | <f2sqty> | <f2pqty> | <f2ldqty> | <f2pgqty> | <f2gsqty> | <f2ptmqty> | <f2bqty> | <f2ebqty> " +
                    "| <f3stype> | <f3scolor> | <f3spost> | <f3sqty> | <f3pqty> | <f3ldqty> | <f3pgqty> | <f3gsqty> | <f3ptmqty> | <f3bqty> | <f3ebqty> "+
                    "| <f4stype> | <f4scolor> | <f4spost> | <f4sqty> | <f4pqty> | <f4ldqty> | <f4pgqty> | <f4gsqty> | <f4ptmqty> | <f4bqty> | <f4ebqty> "+
                    "| <discount> | <adjamt> | <adjcode> | <adjamts> | <adjcodes> " +
                    "| <inlay> | <pushbar> | <silkscreen> | <frontkick> | <backkick> | <fbqty> | <bbqty> | <fbgloc> | <bbgloc> | <singleel> | <led> |";
*/
                MySqlCommand qlcommand = new MySqlCommand("SELECT adj_amt, adj_code, adj_amt_shelving, adj_code_shelving, inlay_color, discount_percent FROM quote_lines WHERE ID = @ID");
                qlcommand.Parameters.AddWithValue("ID", lurow["quote_line_id"].ToString());
                DataSet qlds = MySQLAccess.GetDataSet("172.16.6.60", "wbq_production", "ror", "SomePW23", qlcommand);
                DataRow qlrow = qlds.Tables[0].Rows[0];

                MySqlCommand hdcommand = new MySqlCommand("SELECT remove_pushbar, full_silkscreen_color, kickplate_front, kickplate_back, bumper_guard_qty_front, bumper_guard_qty_back, bumper_guard_location_front, bumper_guard_location_back FROM hd_door_configs WHERE line_up_id = @ID");
                hdcommand.Parameters.AddWithValue("ID", lurow["id"].ToString());
                DataSet hdds = MySQLAccess.GetDataSet("172.16.6.60", "wbq_production", "ror", "SomePW23", hdcommand);

                MySqlCommand lhcommand = new MySqlCommand("SELECT lock01, hinge01, lock02, hinge02, lock03, hinge03, lock04, hinge04, lock05, hinge05, lock06, hinge06, lock07, hinge07, lock08, hinge08, lock09, hinge09, lock10, hinge10, lock11, hinge11, lock12, hinge12, lock13, hinge13, lock14, hinge14, lock15, hinge15, lock16, hinge16, lock17, hinge17, lock18, hinge18, lock19, hinge19, lock20, hinge20 FROM lock_hinges WHERE line_up_id = @ID");
                lhcommand.Parameters.AddWithValue("ID", lurow["id"].ToString());
                DataSet lhds = MySQLAccess.GetDataSet("172.16.6.60", "wbq_production", "ror", "SomePW23", lhcommand);

                MySqlCommand sccommand = new MySqlCommand("SELECT type, shelf_type_id, shelf_color_id, post_color_id, shelf_qty, post_qty, lane_div_qty, perim_grd_qty, glide_sht_qty, price_tag_mld_qty, base_qty, ext_bracket_qty FROM shelf_counts WHERE line_up_id = @ID");
                sccommand.Parameters.AddWithValue("ID", lurow["id"].ToString());
                DataSet scds = MySQLAccess.GetDataSet("172.16.6.60", "wbq_production", "ror", "SomePW23", sccommand);

                #region lineup

                rowentry = rowentry.Replace("<type>", lurow["type_id"].ToString());
                rowentry = rowentry.Replace("<numdoors>", lurow["number_of_doors"].ToString());
                rowentry = rowentry.Replace("<size>", lurow["size_id"].ToString());
                rowentry = rowentry.Replace("<const>", lurow["construction_id"].ToString());
                rowentry = rowentry.Replace("<finish>", lurow["finish_id"].ToString());
                rowentry = rowentry.Replace("<handle>", lurow["handle_id"].ToString());
                rowentry = rowentry.Replace("<light>", lurow["light_id"].ToString());
                rowentry = rowentry.Replace("<skit>", lurow["shelf_kit_id"].ToString());
                rowentry = rowentry.Replace("<scolor>", lurow["shelf_color_id"].ToString());
                rowentry = rowentry.Replace("<spost>", lurow["post_id"].ToString());
                rowentry = rowentry.Replace("<stype>", lurow["shelf_type_id"].ToString());
                rowentry = rowentry.Replace("<singleel>", lurow["single_endlight"].ToString());
                rowentry = rowentry.Replace("<led>", lurow["led_id"].ToString());

                #endregion

                #region quote line

                rowentry = rowentry.Replace("<adjamt>", qlrow["adj_amt"].ToString());
                rowentry = rowentry.Replace("<adjcode>", qlrow["adj_code"].ToString());
                rowentry = rowentry.Replace("<adjamts>", qlrow["adj_amt_shelving"].ToString());
                rowentry = rowentry.Replace("<adjcodes>", qlrow["adj_code_shelving"].ToString());
                rowentry = rowentry.Replace("<inlay>", qlrow["inlay_color"].ToString());
                rowentry = rowentry.Replace("<discount>", qlrow["discount_percent"].ToString());

                #endregion

                #region hd door

                if (hdds.Tables[0].Rows.Count > 0)
                {
                    DataRow hdrow = hdds.Tables[0].Rows[0];
                    rowentry = rowentry.Replace("<pushbar>", hdrow["remove_pushbar"].ToString());
                    rowentry = rowentry.Replace("<silkscreen>", hdrow["full_silkscreen_color"].ToString());
                    rowentry = rowentry.Replace("<frontkick>", hdrow["kickplate_front"].ToString());
                    rowentry = rowentry.Replace("<backkick>", hdrow["kickplate_back"].ToString());
                    rowentry = rowentry.Replace("<fbqty>", hdrow["bumper_guard_qty_front"].ToString());
                    rowentry = rowentry.Replace("<bbqty>", hdrow["bumper_guard_qty_back"].ToString());
                    rowentry = rowentry.Replace("<fbgloc>", hdrow["bumper_guard_location_front"].ToString().Trim());
                    rowentry = rowentry.Replace("<bbgloc>", hdrow["bumper_guard_location_back"].ToString().Trim());
                }
                else
                {
                    rowentry = rowentry.Replace("<pushbar>", "");
                    rowentry = rowentry.Replace("<silkscreen>", "");
                    rowentry = rowentry.Replace("<frontkick>", "");
                    rowentry = rowentry.Replace("<backkick>", "");
                    rowentry = rowentry.Replace("<fbqty>", "");
                    rowentry = rowentry.Replace("<bbqty>", "");
                    rowentry = rowentry.Replace("<fbgloc>", "");
                    rowentry = rowentry.Replace("<bbgloc>", "");
                }

                #endregion

                #region lock/hinge

                if (lhds.Tables[0].Rows.Count > 0)
                {
                    DataRow lhrow = lhds.Tables[0].Rows[0];
                    rowentry = rowentry.Replace("<h1>", lhrow["hinge01"].ToString());
                    rowentry = rowentry.Replace("<l1>", lhrow["lock01"].ToString());
                    rowentry = rowentry.Replace("<h2>", lhrow["hinge02"].ToString());
                    rowentry = rowentry.Replace("<l2>", lhrow["lock02"].ToString());
                    rowentry = rowentry.Replace("<h3>", lhrow["hinge03"].ToString());
                    rowentry = rowentry.Replace("<l3>", lhrow["lock03"].ToString());
                    rowentry = rowentry.Replace("<h4>", lhrow["hinge04"].ToString());
                    rowentry = rowentry.Replace("<l4>", lhrow["lock04"].ToString());
                    rowentry = rowentry.Replace("<h5>", lhrow["hinge05"].ToString());
                    rowentry = rowentry.Replace("<l5>", lhrow["lock05"].ToString());
                    rowentry = rowentry.Replace("<h6>", lhrow["hinge06"].ToString());
                    rowentry = rowentry.Replace("<l6>", lhrow["lock06"].ToString());
                    rowentry = rowentry.Replace("<h7>", lhrow["hinge07"].ToString());
                    rowentry = rowentry.Replace("<l7>", lhrow["lock07"].ToString());
                    rowentry = rowentry.Replace("<h8>", lhrow["hinge08"].ToString());
                    rowentry = rowentry.Replace("<l8>", lhrow["lock08"].ToString());
                    rowentry = rowentry.Replace("<h9>", lhrow["hinge09"].ToString());
                    rowentry = rowentry.Replace("<l9>", lhrow["lock09"].ToString());
                    rowentry = rowentry.Replace("<h10>", lhrow["hinge10"].ToString());
                    rowentry = rowentry.Replace("<l10>", lhrow["lock10"].ToString());
                    rowentry = rowentry.Replace("<h11>", lhrow["hinge11"].ToString());
                    rowentry = rowentry.Replace("<l11>", lhrow["lock11"].ToString());
                    rowentry = rowentry.Replace("<h12>", lhrow["hinge12"].ToString());
                    rowentry = rowentry.Replace("<l12>", lhrow["lock12"].ToString());
                    rowentry = rowentry.Replace("<h13>", lhrow["hinge13"].ToString());
                    rowentry = rowentry.Replace("<l13>", lhrow["lock13"].ToString());
                    rowentry = rowentry.Replace("<h14>", lhrow["hinge14"].ToString());
                    rowentry = rowentry.Replace("<l14>", lhrow["lock14"].ToString());
                    rowentry = rowentry.Replace("<h15>", lhrow["hinge15"].ToString());
                    rowentry = rowentry.Replace("<l15>", lhrow["lock15"].ToString());
                    rowentry = rowentry.Replace("<h16>", lhrow["hinge16"].ToString());
                    rowentry = rowentry.Replace("<l16>", lhrow["lock16"].ToString());
                    rowentry = rowentry.Replace("<h17>", lhrow["hinge17"].ToString());
                    rowentry = rowentry.Replace("<l17>", lhrow["lock17"].ToString());
                    rowentry = rowentry.Replace("<h18>", lhrow["hinge18"].ToString());
                    rowentry = rowentry.Replace("<l18>", lhrow["lock18"].ToString());
                    rowentry = rowentry.Replace("<h19>", lhrow["hinge19"].ToString());
                    rowentry = rowentry.Replace("<l19>", lhrow["lock19"].ToString());
                    rowentry = rowentry.Replace("<h20>", lhrow["hinge20"].ToString());
                    rowentry = rowentry.Replace("<l20>", lhrow["lock20"].ToString());
                }
                else
                {
                    rowentry = rowentry.Replace("<h1>", "");
                    rowentry = rowentry.Replace("<l1>", "");
                    rowentry = rowentry.Replace("<h2>", "");
                    rowentry = rowentry.Replace("<l2>", "");
                    rowentry = rowentry.Replace("<h3>", "");
                    rowentry = rowentry.Replace("<l3>", "");
                    rowentry = rowentry.Replace("<h4>", "");
                    rowentry = rowentry.Replace("<l4>", "");
                    rowentry = rowentry.Replace("<h5>", "");
                    rowentry = rowentry.Replace("<l5>", "");
                    rowentry = rowentry.Replace("<h6>", "");
                    rowentry = rowentry.Replace("<l6>", "");
                    rowentry = rowentry.Replace("<h7>", "");
                    rowentry = rowentry.Replace("<l7>", "");
                    rowentry = rowentry.Replace("<h8>", "");
                    rowentry = rowentry.Replace("<l8>", "");
                    rowentry = rowentry.Replace("<h9>", "");
                    rowentry = rowentry.Replace("<l9>", "");
                    rowentry = rowentry.Replace("<h10>", "");
                    rowentry = rowentry.Replace("<l10>", "");
                    rowentry = rowentry.Replace("<h11>", "");
                    rowentry = rowentry.Replace("<l11>", "");
                    rowentry = rowentry.Replace("<h12>", "");
                    rowentry = rowentry.Replace("<l12>", "");
                    rowentry = rowentry.Replace("<h13>", "");
                    rowentry = rowentry.Replace("<l13>", "");
                    rowentry = rowentry.Replace("<h14>", "");
                    rowentry = rowentry.Replace("<l14>", "");
                    rowentry = rowentry.Replace("<h15>", "");
                    rowentry = rowentry.Replace("<l15>", "");
                    rowentry = rowentry.Replace("<h16>", "");
                    rowentry = rowentry.Replace("<l16>", "");
                    rowentry = rowentry.Replace("<h17>", "");
                    rowentry = rowentry.Replace("<l17>", "");
                    rowentry = rowentry.Replace("<h18>", "");
                    rowentry = rowentry.Replace("<l18>", "");
                    rowentry = rowentry.Replace("<h19>", "");
                    rowentry = rowentry.Replace("<l19>", "");
                    rowentry = rowentry.Replace("<h20>", "");
                    rowentry = rowentry.Replace("<l20>", "");
                }

                #endregion

                #region shelving

                foreach (DataRow srow in scds.Tables[0].Rows)
                {
                    switch (srow["type"].ToString())
                    {
                        case "FirstFrame":
                            rowentry = rowentry.Replace("<f1stype>", srow["shelf_type_id"].ToString());
                            rowentry = rowentry.Replace("<f1scolor>", srow["shelf_color_id"].ToString());
                            rowentry = rowentry.Replace("<f1spost>", srow["post_color_id"].ToString());
                            rowentry = rowentry.Replace("<f1sqty>", srow["shelf_qty"].ToString());
                            rowentry = rowentry.Replace("<f1pqty>", srow["post_qty"].ToString());
                            rowentry = rowentry.Replace("<f1ldqty>", srow["lane_div_qty"].ToString());
                            rowentry = rowentry.Replace("<f1pgqty>", srow["perim_grd_qty"].ToString());
                            rowentry = rowentry.Replace("<f1gsqty>", srow["glide_sht_qty"].ToString());
                            rowentry = rowentry.Replace("<f1ptmqty>", srow["price_tag_mld_qty"].ToString());
                            rowentry = rowentry.Replace("<f1bqty>", srow["base_qty"].ToString());
                            rowentry = rowentry.Replace("<f1ebqty>", srow["ext_bracket_qty"].ToString());
                            break;
                        case "SecondFrame":
                            rowentry = rowentry.Replace("<f2stype>", srow["shelf_type_id"].ToString());
                            rowentry = rowentry.Replace("<f2scolor>", srow["shelf_color_id"].ToString());
                            rowentry = rowentry.Replace("<f2spost>", srow["post_color_id"].ToString());
                            rowentry = rowentry.Replace("<f2sqty>", srow["shelf_qty"].ToString());
                            rowentry = rowentry.Replace("<f2pqty>", srow["post_qty"].ToString());
                            rowentry = rowentry.Replace("<f2ldqty>", srow["lane_div_qty"].ToString());
                            rowentry = rowentry.Replace("<f2pgqty>", srow["perim_grd_qty"].ToString());
                            rowentry = rowentry.Replace("<f2gsqty>", srow["glide_sht_qty"].ToString());
                            rowentry = rowentry.Replace("<f2ptmqty>", srow["price_tag_mld_qty"].ToString());
                            rowentry = rowentry.Replace("<f2bqty>", srow["base_qty"].ToString());
                            rowentry = rowentry.Replace("<f2ebqty>", srow["ext_bracket_qty"].ToString());
                            break;
                        case "ThirdFrame":
                            rowentry = rowentry.Replace("<f3stype>", srow["shelf_type_id"].ToString());
                            rowentry = rowentry.Replace("<f3scolor>", srow["shelf_color_id"].ToString());
                            rowentry = rowentry.Replace("<f3spost>", srow["post_color_id"].ToString());
                            rowentry = rowentry.Replace("<f3sqty>", srow["shelf_qty"].ToString());
                            rowentry = rowentry.Replace("<f3pqty>", srow["post_qty"].ToString());
                            rowentry = rowentry.Replace("<f3ldqty>", srow["lane_div_qty"].ToString());
                            rowentry = rowentry.Replace("<f3pgqty>", srow["perim_grd_qty"].ToString());
                            rowentry = rowentry.Replace("<f3gsqty>", srow["glide_sht_qty"].ToString());
                            rowentry = rowentry.Replace("<f3ptmqty>", srow["price_tag_mld_qty"].ToString());
                            rowentry = rowentry.Replace("<f3bqty>", srow["base_qty"].ToString());
                            rowentry = rowentry.Replace("<f3ebqty>", srow["ext_bracket_qty"].ToString());
                            break;
                        case "FourthFrame":
                            rowentry = rowentry.Replace("<f4stype>", srow["shelf_type_id"].ToString());
                            rowentry = rowentry.Replace("<f4scolor>", srow["shelf_color_id"].ToString());
                            rowentry = rowentry.Replace("<f4spost>", srow["post_color_id"].ToString());
                            rowentry = rowentry.Replace("<f4sqty>", srow["shelf_qty"].ToString());
                            rowentry = rowentry.Replace("<f4pqty>", srow["post_qty"].ToString());
                            rowentry = rowentry.Replace("<f4ldqty>", srow["lane_div_qty"].ToString());
                            rowentry = rowentry.Replace("<f4pgqty>", srow["perim_grd_qty"].ToString());
                            rowentry = rowentry.Replace("<f4gsqty>", srow["glide_sht_qty"].ToString());
                            rowentry = rowentry.Replace("<f4ptmqty>", srow["price_tag_mld_qty"].ToString());
                            rowentry = rowentry.Replace("<f4bqty>", srow["base_qty"].ToString());
                            rowentry = rowentry.Replace("<f4ebqty>", srow["ext_bracket_qty"].ToString());
                            break;
                    }
                }

                rowentry = rowentry.Replace("<f1stype>", "");
                rowentry = rowentry.Replace("<f1scolor>", "");
                rowentry = rowentry.Replace("<f1spost>", "");
                rowentry = rowentry.Replace("<f1sqty>", "");
                rowentry = rowentry.Replace("<f1pqty>", "");
                rowentry = rowentry.Replace("<f1ldqty>", "");
                rowentry = rowentry.Replace("<f1pgqty>", "");
                rowentry = rowentry.Replace("<f1gsqty>", "");
                rowentry = rowentry.Replace("<f1ptmqty>", "");
                rowentry = rowentry.Replace("<f1bqty>", "");
                rowentry = rowentry.Replace("<f1ebqty>", "");
                rowentry = rowentry.Replace("<f2stype>", "");
                rowentry = rowentry.Replace("<f2scolor>", "");
                rowentry = rowentry.Replace("<f2spost>", "");
                rowentry = rowentry.Replace("<f2sqty>", "");
                rowentry = rowentry.Replace("<f2pqty>", "");
                rowentry = rowentry.Replace("<f2ldqty>", "");
                rowentry = rowentry.Replace("<f2pgqty>", "");
                rowentry = rowentry.Replace("<f2gsqty>", "");
                rowentry = rowentry.Replace("<f2ptmqty>", "");
                rowentry = rowentry.Replace("<f2bqty>", "");
                rowentry = rowentry.Replace("<f2ebqty>", "");
                rowentry = rowentry.Replace("<f3stype>", "");
                rowentry = rowentry.Replace("<f3scolor>", "");
                rowentry = rowentry.Replace("<f3spost>", "");
                rowentry = rowentry.Replace("<f3sqty>", "");
                rowentry = rowentry.Replace("<f3pqty>", "");
                rowentry = rowentry.Replace("<f3ldqty>", "");
                rowentry = rowentry.Replace("<f3pgqty>", "");
                rowentry = rowentry.Replace("<f3gsqty>", "");
                rowentry = rowentry.Replace("<f3ptmqty>", "");
                rowentry = rowentry.Replace("<f3bqty>", "");
                rowentry = rowentry.Replace("<f3ebqty>", "");
                rowentry = rowentry.Replace("<f4stype>", "");
                rowentry = rowentry.Replace("<f4scolor>", "");
                rowentry = rowentry.Replace("<f4spost>", "");
                rowentry = rowentry.Replace("<f4sqty>", "");
                rowentry = rowentry.Replace("<f4pqty>", "");
                rowentry = rowentry.Replace("<f4ldqty>", "");
                rowentry = rowentry.Replace("<f4pgqty>", "");
                rowentry = rowentry.Replace("<f4gsqty>", "");
                rowentry = rowentry.Replace("<f4ptmqty>", "");
                rowentry = rowentry.Replace("<f4bqty>", "");
                rowentry = rowentry.Replace("<f4ebqty>", "");

                #endregion

                if (!lines.Contains(rowentry))
                    lines.Add(rowentry);
            }

            lines.Sort();

            int count = 0;
            foreach (string str in lines)
            {
                count++;
                FileStream fstream = new FileStream("C:\\RailsTestScenarios\\test_pricing_" + count.ToString() + ".feature", FileMode.Create);
                StreamWriter swriter = new StreamWriter(fstream);

                swriter.WriteLine(header);
                swriter.WriteLine("Scenario: Pricing Test #" + count.ToString());
                swriter.WriteLine(str);
                swriter.WriteLine();

                swriter.Close();
            }
        }
    }
}
