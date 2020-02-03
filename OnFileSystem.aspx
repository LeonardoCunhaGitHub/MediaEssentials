<%@ Page Title="" Language="C#" MasterPageFile="Layout.Master" AutoEventWireup="true" CodeBehind="OnFileSystem.aspx.cs" Inherits="MediaEssentials.MediaReferences" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
      <!-- Jumbotron -->
    <div class="jumbotron jumbotron-fluid">
        <div class="container">
            <h1 class="display-3">On File System</h1>
            <p class="lead">This module identifies media stored on file system rather than on Database.</p>
        </div>
    </div>
    
    <%--breadcrumb--%>
    <div class="container">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><asp:HyperLink ID="lnkDashboard" runat="server">Dashboard</asp:HyperLink></li>
                <li class="breadcrumb-item active" aria-current="page">On File System</li>
            </ol>
        </nav>
    </div>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" ></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div class="container">
                <div class="form-row">
                    <div class="form-group col-lg-6 ">
                        <div class="card">
                            <div class="card-header">
                                Filter Options
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <label for="ddDataBase" class="col-form-label">Select the Database</label>
                                        <asp:DropDownList ID="ddDataBase" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddDataBase_OnSelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <label  for="ddMediaFolders" class="col-form-label">Select the Media Folder</label>
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
                                
                                <div class="alert alert-secondary" role="alert">

                                    <div class="checkbox">
                                        <label>
                                            <asp:CheckBox ID="chkUploadFileToDB" runat="server" />
                                            Upload File to DB
                                        </label>
                                    </div>

                                </div>
                                

                                <div class="row">
                                    <div class="form-group col-lg-12">
                                        <asp:LinkButton ID="btnFindMedia" runat="server" CssClass="btn btn-outline-primary" OnClick="btnFindMedia_OnClick" >Find Media</asp:LinkButton>
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
