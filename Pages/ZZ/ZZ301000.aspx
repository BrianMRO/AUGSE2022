<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ZZ301000.aspx.cs" Inherits="Page_ZZ301000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="AUGSE2022.AUGOrderEntry"
        PrimaryView="Document"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Document" Width="100%" Height="120px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXLayoutRule ControlSize="SM" LabelsWidth="S" runat="server" ID="CstPXLayoutRule7" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector3" DataField="OrderCD" ></px:PXSelector>
			<px:PXDropDown Enabled="False" runat="server" ID="CstPXDropDown5" DataField="Status" ></px:PXDropDown>
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit4" DataField="OrderDate" ></px:PXDateTimeEdit>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule8" ColumnSpan="2" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit2" DataField="Descr" ></px:PXTextEdit>
			<px:PXLayoutRule LabelsWidth="S" ControlSize="SM" runat="server" ID="CstPXLayoutRule6" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector10" DataField="WorkgroupID" ></px:PXSelector>
			<px:PXSelector runat="server" ID="CstPXSelector9" DataField="OwnerID" ></px:PXSelector></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Details">
				<Template>
					<px:PXGrid Width="100%" SkinID="Details" runat="server" ID="CstPXGrid1">
						<Levels>
							<px:PXGridLevel DataMember="Transactions" >
								<Columns>
									<px:PXGridColumn CommitChanges="True" DataField="InventoryID" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryItem__Descr" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Quantity" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn DataField="BaseUnit" Width="72" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize>
						<AutoSize MinHeight="100" ></AutoSize></px:PXGrid></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Approvals">
				<Template>
					<px:PXGrid Caption="" SkinID="DetailsInTab" Width="100%" runat="server" ID="CstPXGridAprv" DataSourceID="ds">
						<Levels>
							<px:PXGridLevel DataMember="Approval" >
								<Columns>
									<px:PXGridColumn DataField="ApproverEmployee__AcctName" Width="160" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ApproverEmployee__AcctCD" Width="160" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ApprovedByEmployee__AcctName" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ApprovedByEmployee__AcctCD" Width="160" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ApproveDate" Width="90" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Status" Width="90" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Reason" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="WorkgroupID" Width="150" ></px:PXGridColumn></Columns>
								<RowTemplate>
									<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit1Aprv" DataField="ApproveDate" ></px:PXDateTimeEdit>
									<px:PXTextEdit runat="server" ID="CstPXTextEdit2Aprv" DataField="ApprovedByEmployee__AcctCD" ></px:PXTextEdit>
									<px:PXTextEdit runat="server" ID="CstPXTextEdit3Aprv" DataField="ApprovedByEmployee__AcctName" ></px:PXTextEdit>
									<px:PXTextEdit runat="server" ID="CstPXTextEdit4Aprv" DataField="ApproverEmployee__AcctCD" ></px:PXTextEdit>
									<px:PXTextEdit runat="server" ID="CstPXTextEdit5Aprv" DataField="ApproverEmployee__AcctName" ></px:PXTextEdit>
									<px:PXDropDown runat="server" ID="CstPXDropDown6Aprv" DataField="Status" ></px:PXDropDown>
									<px:PXSelector runat="server" ID="CstPXSelector7Aprv" DataField="WorkgroupID" ></px:PXSelector>
									<px:PXTextEdit runat="server" ID="CstPXTextEdit8Aprv" DataField="Reason" ></px:PXTextEdit></RowTemplate></px:PXGridLevel></Levels>
						<AutoSize Container="Window" MinHeight="150" Enabled="True" ></AutoSize>
						<Mode AllowAddNew="False" ></Mode>
						<Mode AllowDelete="False" ></Mode>
						<Mode AllowUpdate="False" ></Mode></px:PXGrid></Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
	</px:PXTab>
	<px:PXSmartPanel ID="panelReason" runat="server" Caption="Enter Reason" CaptionVisible="true" LoadOnDemand="true" Key="ReasonApproveRejectParams"
  AutoCallBack-Enabled="true" AutoCallBack-Command="Refresh" CallBackMode-CommitChanges="True" Width="600px"
  CallBackMode-PostData="Page" AcceptButtonID="btnReasonOk" CancelButtonID="btnReasonCancel" AllowResize="False">
  <px:PXFormView ID="PXFormViewPanelReason" runat="server" DataSourceID="ds" CaptionVisible="False" DataMember="ReasonApproveRejectParams">
    <ContentStyle BackColor="Transparent" BorderStyle="None" Width="100%" Height="100%"  CssClass="" ></ContentStyle> 
    <Template>
      <px:PXLayoutRule ID="PXLayoutRulePanelReason" runat="server" StartColumn="True" ></px:PXLayoutRule>
      <px:PXPanel ID="PXPanelReason" runat="server" RenderStyle="Simple" >
        <px:PXLayoutRule ID="PXLayoutRuleReason" runat="server" StartColumn="True" SuppressLabel="True" ></px:PXLayoutRule>
        <px:PXTextEdit ID="edReason" runat="server" DataField="Reason" TextMode="MultiLine" LabelWidth="0" Height="200px" Width="600px" CommitChanges="True" ></px:PXTextEdit>
      </px:PXPanel>
      <px:PXPanel ID="PXPanelReasonButtons" runat="server" SkinID="Buttons">
        <px:PXButton ID="btnReasonOk" runat="server" Text="OK" DialogResult="Yes" CommandSourceID="ds" ></px:PXButton>
        <px:PXButton ID="btnReasonCancel" runat="server" Text="Cancel" DialogResult="No" CommandSourceID="ds" ></px:PXButton>
      </px:PXPanel>
    </Template>
  </px:PXFormView>
</px:PXSmartPanel>
</asp:Content>