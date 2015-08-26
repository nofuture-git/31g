<%@ Application Language="C#" Debug="true" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Web" %>
<%@ Import namespace="System.Web.Mvc" %>
<%@ Import namespace="System.Web.Routing" %>
<script runat="server">
	protected void Application_Start()
	{
		RegisterRoutes(RouteTable.Routes);
	}
</script>
<script runat="server">
	public static void RegisterRoutes(RouteCollection routes)
	{
		routes.MapRoute("ViewCase", "Case/GetCaseDetails/{caseID}", 
			new { controller = "Case", action = "GetCaseDetails", caseID = UrlParameter.Optional }, 
			new { caseID = "[0-9]+" });
	}
</script>

