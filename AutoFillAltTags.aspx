<%@ Page Title="" Language="C#" MasterPageFile="Layout.Master" AutoEventWireup="true" CodeBehind="AutoFillAltTags.aspx.cs" Inherits="MediaEssentials.AutoFillAltTags" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <!-- Jumbotron -->
    <div class="jumbotron jumbotron-fluid">
        <div class="container">
            <h1 class="display-3">Auto-fill Alt Tags</h1>
            <p class="lead">This will auto-fill the alt tags of media items with the name of the item.</p>
            <p class="lead">All languages installed will be considered.</p>
        </div>
    </div>
    
    <%--breadcrumb--%>
    <div class="container">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><asp:HyperLink ID="lnkDashboard" runat="server">Dashboard</asp:HyperLink></li>
                <li class="breadcrumb-item active" aria-current="page">Auto-fill Alt Tags</li>
            </ol>
        </nav>
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAutofill" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="form-group col-lg-6 ">
                        <div class="card">
                            <div class="card-header">
                                Filter Options
                            </div>
                            <div class="card-body">
                                <div class="row">

                                    <div class="form-group col-lg-12">
                                        <label>Select the Database</label>
                                        <asp:DropDownList ID="ddDataBase" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddDataBase_OnSelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <label>Which media folder do you want to export?</label>
                                        <asp:DropDownList ID="ddMediaFolders" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkIncludeSystemFolder" runat="server" />
                                                Include System folder?
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="chkIncludeSubFolders" runat="server" />
                                                Include all sub folders?
                                            </label>
                                        </div>
                                    </div>
                                </div>
     
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <div class="alert alert-danger" role="alert">
                                            <div class="checkbox">
                                                <label>
                                                    <asp:CheckBox ID="chkOnlyEmptyAltTags" runat="server" />
                                                    Only update empty ALT tags!
                                                </label>
                                            </div>
                                        </div>
                                        <asp:LinkButton ID="btnAutofill" runat="server" CssClass="btn btn-outline-primary" OnClick="btnAutofill_OnClick">Auto-fill ALT Tags</asp:LinkButton>


                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="form-group col-lg-6">
                        <div class="card">
                            <div class="card-header">
                                Output
                            </div>
                            <div class="card-body scroll">
                                <asp:Label ID="lbOutput" runat="server" Font-Size="8"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>


            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
