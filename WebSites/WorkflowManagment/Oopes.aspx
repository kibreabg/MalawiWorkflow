<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="Oopes.aspx.cs" Inherits="Oopes" %>


<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" Runat="Server">
    <div class="col-sm-12">
								<div class="text-center error-box">
									<h1 class="error-text tada animated"><i class="fa fa-times-circle text-danger error-icon-shadow"></i> Error </h1>
									<h2 class="font-xl"><strong>Oooops, Something went wrong!</strong></h2>
									<br>
									<p class="lead semi-bold">
										<strong>You have experienced a technical error. We apologize. The error has been sent to administrators for correction!</strong><br><br>
									
									</p>
									<%--<ul class="error-search text-left font-md">
							    
							            <li><a href="javascript:void(0);"><small <i class="fa fa-mail-forward"></i></small></a></li>
                                       
                                       
							        </ul>--%>
                                        <%--Error Message : <asp:Label ID="lblError" runat="server" Text=""></asp:Label><br />
                                        Inner Exception : <asp:Label ID="lblInnerException" runat="server" Text=""></asp:Label><br />
                                        Source : <asp:Label ID="lblSource" runat="server" Text=""></asp:Label><br />--%>
								</div>
				
							</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" Runat="Server">
    
</asp:Content>
