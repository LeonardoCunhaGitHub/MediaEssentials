﻿<%@ Page Title="" Language="C#" MasterPageFile="Layout.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="MediaEssentialsMVC.MediaEssentials" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <!-- Jumbotron -->
    <div class="jumbotron jumbotron-fluid">
        <div class="container">
            <h1 class="display-3">Media Essentials</h1>
            <p class="lead">An essential Sitecore Module to manage your media library.</p>
        </div>
    </div>


    <div class="container">

        <!-- first row -->
        <div class="row">
            <div class="col-md-4">
                <div class="card mb-5">
                    <div class="card-header">
                        Export Media
                    </div>
                    <div class="card-body">
                        <p class="card-text">Export Sitecore Media Library and download it as a ZIP file.</p>
                        <a class="btn btn-outline-primary btn-block" runat="server" href="~/sitecore/admin/mediaessentials/ExportMedia.aspx" role="button">Go &raquo;</a>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card mb-5">
                    <div class="card-header">
                        Unused Media
                    </div>
                    <div class="card-body">
                        <p class="card-text">Identify all unused media and move them to recyle bin or delete.</p>
                        <a class="btn btn-outline-primary btn-block" runat="server" href="~/sitecore/admin/mediaessentials/UnusedMedia.aspx" role="button">Go &raquo;</a>
                    </div>
                </div>
            </div>


            <div class="col-md-4">
                <div class="card mb-5">
                    <div class="card-header">
                        Auto-fill Alt Tags
                    </div>
                    <div class="card-body">
                        <p class="card-text">Fill in all the Media Tags with the name of the media item.</p>
                        <a class="btn btn-outline-primary btn-block" runat="server" href="~/sitecore/admin/mediaessentials/AutoFillAltTags.aspx" role="button">Go &raquo;</a>
                    </div>
                </div>
            </div>

        </div>

        <!-- second row -->
        <div class="row">
            <div class="col-md-4">
                <div class="card mb-5">
                    <div class="card-header">
                        On File System
                    </div>
                    <div class="card-body">
                        <p class="card-text">Identifies media stored on file system rather than on Database.</p>
                        <a class="btn btn-outline-primary btn-block" runat="server" href="~/sitecore/admin/mediaessentials/OnFileSystem.aspx" role="button">Go &raquo;</a>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card mb-5">
                    <div class="card-header">
                        Date Uploaded
                    </div>
                    <div class="card-body">
                        <p class="card-text">Find all media uploaded based on a specific date.</p>
                        <a class="btn btn-outline-primary btn-block" runat="server" href="~/sitecore/admin/mediaessentials/dateUploaded.aspx" role="button">Go &raquo;</a>
                    </div>
                </div>
            </div>


            <div class="col-md-4">
                <div class="card mb-5">
                    <div class="card-header">
                        Media Size
                    </div>
                    <div class="card-body">
                        <p class="card-text">Identifies media greater/less than specific size.</p>
                        <a class="btn btn-outline-primary btn-block" runat="server" href="~/sitecore/admin/mediaessentials/MediaSize.aspx" role="button">Go &raquo;</a>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>


