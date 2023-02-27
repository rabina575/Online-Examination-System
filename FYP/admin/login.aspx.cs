using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class admin_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    string s = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
    
    protected void btn_login_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {

            using (SqlConnection con = new SqlConnection(s))
            {
                SqlCommand cmd = new SqlCommand("spAdminlogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@admin_email", txt_email.Text);
                cmd.Parameters.AddWithValue("@password", txt_pass.Text);
                try
                {
                    con.Open();
                    int value = (int)cmd.ExecuteScalar();
                    if (value == 1)
                    {
                        if (chk_remember.Checked)
                        {
                            HttpCookie user = new HttpCookie("admin_cookies");
                            user["adminemail"] = txt_email.Text;
                            user.Expires = DateTime.Now.AddYears(3); 
                            Response.Cookies.Add(user);
                        }
                        else
                        {
                            Session["adminemail"] = txt_email.Text;
                        }
                        Response.Redirect("~/admin/Index.aspx");
                    }
                    else
                    {
                        pnl_warning.Visible = true;
                        lbl_warning.Text = "Use correct email and password</br>";
                    }

                }
                catch (Exception ex)
                {
                    pnl_warning.Visible = true;
                    lbl_warning.Text = "Something went wrong! Contact your devloper </br>" + ex.Message;
                }
            }
        }
        else
        {
            pnl_warning.Visible = true;
            lbl_warning.Text = "Please fill all the requirements";
        }

    }
}