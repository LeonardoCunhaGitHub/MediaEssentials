<%@ Page Title="" Language="C#" MasterPageFile="Layout.Master" AutoEventWireup="true" CodeBehind="UnusedMedia.aspx.cs" Inherits="MediaEssentialsSitecoreModule.UnusedMedia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <!-- Jumbotron -->
    <div class="jumbotron jumbotron-fluid">
        <div class="container">
            <h1 class="display-3">Unused Media</h1>
            <p class="lead">This module is going to identify all unused media and give you the option to move all to recycle bin or delete them.</p>
        </div>
    </div>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" ></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div class="container">
                <div class="form-row">
                    <div class="form-group col-lg-6 ">
                        <div class="card">
                            <div class="card-header">
                                Export
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <label for="ddDataBase" class="col-form-label">Select the Database</label>
                                        <asp:DropDownList ID="ddDataBase" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddDataBase_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <label  for="ddMediaFolders" class="col-form-label">Which media folder do you want to export?</label>
                                        <asp:DropDownList ID="ddMediaFolders" runat="server" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <div class="form-check">
                                            <label>
                                                <asp:CheckBox ID="chkIncludeSystemFolder" runat="server" />
                                                Include System folder?
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <div class="form-check">
                                            <label>
                                                <asp:CheckBox ID="chkIncludeSubFolders" runat="server" />
                                                Include all sub folders?
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                    
                                        <asp:LinkButton ID="btnUnusedMedia" runat="server" CssClass="btn btn-outline-primary" OnClick="btnUnusedMedia_OnClick" >Find Unused Media</asp:LinkButton>
                                            
                                        <asp:LinkButton ID="btnRecycleBin" runat="server" CssClass="btn btn-link" OnClick="btnRecycleBin_OnClick" >Move to Recyle Bin</asp:LinkButton>
                                            
                                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-link" OnClick="btnDelete_OnClick" >Delete Unsed Media</asp:LinkButton>
                                   
                                    
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
                            <div class="card-body">
                                <asp:Label ID="lbOutput" runat="server" Font-Size="8"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>




            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



