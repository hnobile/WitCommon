<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddThisButton.ascx.cs" Inherits="WIT.Common.AddThisButton.UserControls.AddThisButton" %>
<div class="addthis_toolbox addthis_default_style" 
    <%= (!string.IsNullOrEmpty(URL)? "addthis:url='" + URL + "'" : ((Request.Url != null)? "addthis:url='" + Request.Url.ToString() + "'" : string.Empty)) %>
    <%= !string.IsNullOrEmpty(Title)? "addthis:title='" + Title + "'" : string.Empty %>
    <%= !string.IsNullOrEmpty(SelectedLanguage)? "addthis:ui_language='" + SelectedLanguage + "'" : string.Empty %>>
        <%= BuildMenuServices() %>
        <%= !string.IsNullOrEmpty(Separator)? "<span class='addthis_separator'>" + Separator + "</span>" : string.Empty %>
        <a href="http://www.addthis.com/bookmark.php?v=250&pub=xa-4a60bb071de35881" class="addthis_button_expanded"><%= TextMore %></a>
</div>

