using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.RegionalData
{
    public class GeoFips : BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
        private static List<GeoFips> _values;
        public static List<GeoFips> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<GeoFips>
                           {
                           
                           new GeoFips
                           {
                               Description = "0 All counties",
                               Val = "COUNTY",
                           },
                           new GeoFips
                           {
                               Description = "0 All states, regions, and US",
                               Val = "STATE",
                           },
                           new GeoFips
                           {
                               Description = "0 All metropolitan areas",
                               Val = "MSA",
                           },
                           new GeoFips
                           {
                               Description = "4 Newport News (Independent City), VA ",
                               Val = "51700",
                           },
                           new GeoFips
                           {
                               Description = "4 Norfolk (Independent City), VA ",
                               Val = "51710",
                           },
                           new GeoFips
                           {
                               Description = "4 Portsmouth (Independent City), VA ",
                               Val = "51740",
                           },
                           new GeoFips
                           {
                               Description = "4 Richmond (Independent City), VA ",
                               Val = "51760",
                           },
                           new GeoFips
                           {
                               Description = "4 Roanoke (Independent City), VA ",
                               Val = "51770",
                           },
                           new GeoFips
                           {
                               Description = "4 Suffolk (Independent City), VA ",
                               Val = "51800",
                           },
                           new GeoFips
                           {
                               Description = "4 Virginia Beach (Independent City), VA ",
                               Val = "51810",
                           },
                           new GeoFips
                           {
                               Description = "4 Albemarle + Charlottesville, VA ",
                               Val = "51901",
                           },
                           new GeoFips
                           {
                               Description = "4 Alleghany + Covington, VA ",
                               Val = "51903",
                           },
                           new GeoFips
                           {
                               Description = "4 Augusta, Staunton + Waynesboro, VA ",
                               Val = "51907",
                           },
                           new GeoFips
                           {
                               Description = "4 Bedford + Bedford City, VA ",
                               Val = "51909",
                           },
                           new GeoFips
                           {
                               Description = "4 Campbell + Lynchburg, VA ",
                               Val = "51911",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll + Galax, VA ",
                               Val = "51913",
                           },
                           new GeoFips
                           {
                               Description = "4 Dinwiddie, Colonial Heights + Petersburg, VA ",
                               Val = "51918",
                           },
                           new GeoFips
                           {
                               Description = "4 Fairfax, Fairfax City + Falls Church, VA ",
                               Val = "51919",
                           },
                           new GeoFips
                           {
                               Description = "4 Frederick + Winchester, VA ",
                               Val = "51921",
                           },
                           new GeoFips
                           {
                               Description = "4 Greensville + Emporia, VA ",
                               Val = "51923",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry + Martinsville, VA ",
                               Val = "51929",
                           },
                           new GeoFips
                           {
                               Description = "4 James City + Williamsburg, VA ",
                               Val = "51931",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery + Radford, VA ",
                               Val = "51933",
                           },
                           new GeoFips
                           {
                               Description = "4 Pittsylvania + Danville, VA ",
                               Val = "51939",
                           },
                           new GeoFips
                           {
                               Description = "4 Prince George + Hopewell, VA ",
                               Val = "51941",
                           },
                           new GeoFips
                           {
                               Description = "4 Prince William, Manassas + Manassas Park, VA ",
                               Val = "51942",
                           },
                           new GeoFips
                           {
                               Description = "4 Roanoke + Salem, VA ",
                               Val = "51944",
                           },
                           new GeoFips
                           {
                               Description = "4 Rockbridge, Buena Vista + Lexington, VA ",
                               Val = "51945",
                           },
                           new GeoFips
                           {
                               Description = "4 Rockingham + Harrisonburg, VA ",
                               Val = "51947",
                           },
                           new GeoFips
                           {
                               Description = "4 Southampton + Franklin, VA ",
                               Val = "51949",
                           },
                           new GeoFips
                           {
                               Description = "4 Spotsylvania + Fredericksburg, VA ",
                               Val = "51951",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington + Bristol, VA ",
                               Val = "51953",
                           },
                           new GeoFips
                           {
                               Description = "4 Wise + Norton, VA ",
                               Val = "51955",
                           },
                           new GeoFips
                           {
                               Description = "4 York + Poquoson, VA ",
                               Val = "51958",
                           },
                           new GeoFips
                           {
                               Description = "3 Washington",
                               Val = "53000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, WA ",
                               Val = "53001",
                           },
                           new GeoFips
                           {
                               Description = "4 Asotin, WA ",
                               Val = "53003",
                           },
                           new GeoFips
                           {
                               Description = "4 Benton, WA ",
                               Val = "53005",
                           },
                           new GeoFips
                           {
                               Description = "4 Chelan, WA ",
                               Val = "53007",
                           },
                           new GeoFips
                           {
                               Description = "4 Clallam, WA ",
                               Val = "53009",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, WA ",
                               Val = "53011",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbia, WA ",
                               Val = "53013",
                           },
                           new GeoFips
                           {
                               Description = "4 Cowlitz, WA ",
                               Val = "53015",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, WA ",
                               Val = "53017",
                           },
                           new GeoFips
                           {
                               Description = "4 Ferry, WA ",
                               Val = "53019",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, WA ",
                               Val = "53021",
                           },
                           new GeoFips
                           {
                               Description = "4 Garfield, WA ",
                               Val = "53023",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, WA ",
                               Val = "53025",
                           },
                           new GeoFips
                           {
                               Description = "4 Grays Harbor, WA ",
                               Val = "53027",
                           },
                           new GeoFips
                           {
                               Description = "4 Island, WA ",
                               Val = "53029",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, WA ",
                               Val = "53031",
                           },
                           new GeoFips
                           {
                               Description = "4 King, WA ",
                               Val = "53033",
                           },
                           new GeoFips
                           {
                               Description = "4 Kitsap, WA ",
                               Val = "53035",
                           },
                           new GeoFips
                           {
                               Description = "4 Kittitas, WA ",
                               Val = "53037",
                           },
                           new GeoFips
                           {
                               Description = "4 Klickitat, WA ",
                               Val = "53039",
                           },
                           new GeoFips
                           {
                               Description = "4 Lewis, WA ",
                               Val = "53041",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, WA ",
                               Val = "53043",
                           },
                           new GeoFips
                           {
                               Description = "4 Mason, WA ",
                               Val = "53045",
                           },
                           new GeoFips
                           {
                               Description = "4 Okanogan, WA ",
                               Val = "53047",
                           },
                           new GeoFips
                           {
                               Description = "4 Pacific, WA ",
                               Val = "53049",
                           },
                           new GeoFips
                           {
                               Description = "4 Pend Oreille, WA ",
                               Val = "53051",
                           },
                           new GeoFips
                           {
                               Description = "4 Pierce, WA ",
                               Val = "53053",
                           },
                           new GeoFips
                           {
                               Description = "4 San Juan, WA ",
                               Val = "53055",
                           },
                           new GeoFips
                           {
                               Description = "4 Skagit, WA ",
                               Val = "53057",
                           },
                           new GeoFips
                           {
                               Description = "4 Skamania, WA ",
                               Val = "53059",
                           },
                           new GeoFips
                           {
                               Description = "4 Snohomish, WA ",
                               Val = "53061",
                           },
                           new GeoFips
                           {
                               Description = "4 Spokane, WA ",
                               Val = "53063",
                           },
                           new GeoFips
                           {
                               Description = "4 Stevens, WA ",
                               Val = "53065",
                           },
                           new GeoFips
                           {
                               Description = "4 Thurston, WA ",
                               Val = "53067",
                           },
                           new GeoFips
                           {
                               Description = "4 Wahkiakum, WA ",
                               Val = "53069",
                           },
                           new GeoFips
                           {
                               Description = "4 Walla Walla, WA ",
                               Val = "53071",
                           },
                           new GeoFips
                           {
                               Description = "4 Whatcom, WA ",
                               Val = "53073",
                           },
                           new GeoFips
                           {
                               Description = "4 Whitman, WA ",
                               Val = "53075",
                           },
                           new GeoFips
                           {
                               Description = "4 Yakima, WA ",
                               Val = "53077",
                           },
                           new GeoFips
                           {
                               Description = "3 West Virginia",
                               Val = "54000",
                           },
                           new GeoFips
                           {
                               Description = "4 Barbour, WV ",
                               Val = "54001",
                           },
                           new GeoFips
                           {
                               Description = "4 Berkeley, WV ",
                               Val = "54003",
                           },
                           new GeoFips
                           {
                               Description = "4 Boone, WV ",
                               Val = "54005",
                           },
                           new GeoFips
                           {
                               Description = "4 Braxton, WV ",
                               Val = "54007",
                           },
                           new GeoFips
                           {
                               Description = "4 Brooke, WV ",
                               Val = "54009",
                           },
                           new GeoFips
                           {
                               Description = "4 Cabell, WV ",
                               Val = "54011",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, WV ",
                               Val = "54013",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, WV ",
                               Val = "54015",
                           },
                           new GeoFips
                           {
                               Description = "4 Doddridge, WV ",
                               Val = "54017",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, WV ",
                               Val = "54019",
                           },
                           new GeoFips
                           {
                               Description = "4 Gilmer, WV ",
                               Val = "54021",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, WV ",
                               Val = "54023",
                           },
                           new GeoFips
                           {
                               Description = "4 Greenbrier, WV ",
                               Val = "54025",
                           },
                           new GeoFips
                           {
                               Description = "4 Hampshire, WV ",
                               Val = "54027",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, WV ",
                               Val = "54029",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardy, WV ",
                               Val = "54031",
                           },
                           new GeoFips
                           {
                               Description = "4 Harrison, WV ",
                               Val = "54033",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, WV ",
                               Val = "54035",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, WV ",
                               Val = "54037",
                           },
                           new GeoFips
                           {
                               Description = "4 Kanawha, WV ",
                               Val = "54039",
                           },
                           new GeoFips
                           {
                               Description = "4 Lewis, WV ",
                               Val = "54041",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, WV ",
                               Val = "54043",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, WV ",
                               Val = "54045",
                           },
                           new GeoFips
                           {
                               Description = "4 McDowell, WV ",
                               Val = "54047",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, WV ",
                               Val = "54049",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, WV ",
                               Val = "54051",
                           },
                           new GeoFips
                           {
                               Description = "4 Mason, WV ",
                               Val = "54053",
                           },
                           new GeoFips
                           {
                               Description = "4 Mercer, WV ",
                               Val = "54055",
                           },
                           new GeoFips
                           {
                               Description = "4 Mineral, WV ",
                               Val = "54057",
                           },
                           new GeoFips
                           {
                               Description = "4 Mingo, WV ",
                               Val = "54059",
                           },
                           new GeoFips
                           {
                               Description = "4 Monongalia, WV ",
                               Val = "54061",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, WV ",
                               Val = "54063",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, WV ",
                               Val = "54065",
                           },
                           new GeoFips
                           {
                               Description = "4 Nicholas, WV ",
                               Val = "54067",
                           },
                           new GeoFips
                           {
                               Description = "4 Ohio, WV ",
                               Val = "54069",
                           },
                           new GeoFips
                           {
                               Description = "4 Pendleton, WV ",
                               Val = "54071",
                           },
                           new GeoFips
                           {
                               Description = "4 Pleasants, WV ",
                               Val = "54073",
                           },
                           new GeoFips
                           {
                               Description = "4 Pocahontas, WV ",
                               Val = "54075",
                           },
                           new GeoFips
                           {
                               Description = "4 Preston, WV ",
                               Val = "54077",
                           },
                           new GeoFips
                           {
                               Description = "4 Putnam, WV ",
                               Val = "54079",
                           },
                           new GeoFips
                           {
                               Description = "4 Raleigh, WV ",
                               Val = "54081",
                           },
                           new GeoFips
                           {
                               Description = "4 Randolph, WV ",
                               Val = "54083",
                           },
                           new GeoFips
                           {
                               Description = "4 Ritchie, WV ",
                               Val = "54085",
                           },
                           new GeoFips
                           {
                               Description = "4 Roane, WV ",
                               Val = "54087",
                           },
                           new GeoFips
                           {
                               Description = "4 Summers, WV ",
                               Val = "54089",
                           },
                           new GeoFips
                           {
                               Description = "4 Taylor, WV ",
                               Val = "54091",
                           },
                           new GeoFips
                           {
                               Description = "4 Tucker, WV ",
                               Val = "54093",
                           },
                           new GeoFips
                           {
                               Description = "4 Tyler, WV ",
                               Val = "54095",
                           },
                           new GeoFips
                           {
                               Description = "4 Upshur, WV ",
                               Val = "54097",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, WV ",
                               Val = "54099",
                           },
                           new GeoFips
                           {
                               Description = "4 Webster, WV ",
                               Val = "54101",
                           },
                           new GeoFips
                           {
                               Description = "4 Wetzel, WV ",
                               Val = "54103",
                           },
                           new GeoFips
                           {
                               Description = "4 Wirt, WV ",
                               Val = "54105",
                           },
                           new GeoFips
                           {
                               Description = "4 Wood, WV ",
                               Val = "54107",
                           },
                           new GeoFips
                           {
                               Description = "4 Wyoming, WV ",
                               Val = "54109",
                           },
                           new GeoFips
                           {
                               Description = "3 Wisconsin",
                               Val = "55000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, WI ",
                               Val = "55001",
                           },
                           new GeoFips
                           {
                               Description = "4 Ashland, WI ",
                               Val = "55003",
                           },
                           new GeoFips
                           {
                               Description = "4 Barron, WI ",
                               Val = "55005",
                           },
                           new GeoFips
                           {
                               Description = "4 Bayfield, WI ",
                               Val = "55007",
                           },
                           new GeoFips
                           {
                               Description = "4 Brown, WI ",
                               Val = "55009",
                           },
                           new GeoFips
                           {
                               Description = "4 Buffalo, WI ",
                               Val = "55011",
                           },
                           new GeoFips
                           {
                               Description = "4 Burnett, WI ",
                               Val = "55013",
                           },
                           new GeoFips
                           {
                               Description = "4 Calumet, WI ",
                               Val = "55015",
                           },
                           new GeoFips
                           {
                               Description = "4 Chippewa, WI ",
                               Val = "55017",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, WI ",
                               Val = "55019",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbia, WI ",
                               Val = "55021",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, WI ",
                               Val = "55023",
                           },
                           new GeoFips
                           {
                               Description = "4 Dane, WI ",
                               Val = "55025",
                           },
                           new GeoFips
                           {
                               Description = "4 Dodge, WI ",
                               Val = "55027",
                           },
                           new GeoFips
                           {
                               Description = "4 Door, WI ",
                               Val = "55029",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, WI ",
                               Val = "55031",
                           },
                           new GeoFips
                           {
                               Description = "4 Dunn, WI ",
                               Val = "55033",
                           },
                           new GeoFips
                           {
                               Description = "4 Eau Claire, WI ",
                               Val = "55035",
                           },
                           new GeoFips
                           {
                               Description = "4 Florence, WI ",
                               Val = "55037",
                           },
                           new GeoFips
                           {
                               Description = "4 Fond du Lac, WI ",
                               Val = "55039",
                           },
                           new GeoFips
                           {
                               Description = "4 Forest, WI ",
                               Val = "55041",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, WI ",
                               Val = "55043",
                           },
                           new GeoFips
                           {
                               Description = "4 Green, WI ",
                               Val = "55045",
                           },
                           new GeoFips
                           {
                               Description = "4 Green Lake, WI ",
                               Val = "55047",
                           },
                           new GeoFips
                           {
                               Description = "4 Iowa, WI ",
                               Val = "55049",
                           },
                           new GeoFips
                           {
                               Description = "4 Iron, WI ",
                               Val = "55051",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, WI ",
                               Val = "55053",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, WI ",
                               Val = "55055",
                           },
                           new GeoFips
                           {
                               Description = "4 Juneau, WI ",
                               Val = "55057",
                           },
                           new GeoFips
                           {
                               Description = "4 Kenosha, WI ",
                               Val = "55059",
                           },
                           new GeoFips
                           {
                               Description = "4 Kewaunee, WI ",
                               Val = "55061",
                           },
                           new GeoFips
                           {
                               Description = "4 La Crosse, WI ",
                               Val = "55063",
                           },
                           new GeoFips
                           {
                               Description = "4 Lafayette, WI ",
                               Val = "55065",
                           },
                           new GeoFips
                           {
                               Description = "4 Langlade, WI ",
                               Val = "55067",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, WI ",
                               Val = "55069",
                           },
                           new GeoFips
                           {
                               Description = "4 Manitowoc, WI ",
                               Val = "55071",
                           },
                           new GeoFips
                           {
                               Description = "4 Marathon, WI ",
                               Val = "55073",
                           },
                           new GeoFips
                           {
                               Description = "4 Marinette, WI ",
                               Val = "55075",
                           },
                           new GeoFips
                           {
                               Description = "4 Marquette, WI ",
                               Val = "55077",
                           },
                           new GeoFips
                           {
                               Description = "4 Menominee, WI ",
                               Val = "55078",
                           },
                           new GeoFips
                           {
                               Description = "4 Milwaukee, WI ",
                               Val = "55079",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, WI ",
                               Val = "55081",
                           },
                           new GeoFips
                           {
                               Description = "4 Oconto, WI ",
                               Val = "55083",
                           },
                           new GeoFips
                           {
                               Description = "4 Oneida, WI ",
                               Val = "55085",
                           },
                           new GeoFips
                           {
                               Description = "4 Outagamie, WI ",
                               Val = "55087",
                           },
                           new GeoFips
                           {
                               Description = "4 Ozaukee, WI ",
                               Val = "55089",
                           },
                           new GeoFips
                           {
                               Description = "4 Pepin, WI ",
                               Val = "55091",
                           },
                           new GeoFips
                           {
                               Description = "4 Pierce, WI ",
                               Val = "55093",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, WI ",
                               Val = "55095",
                           },
                           new GeoFips
                           {
                               Description = "4 Portage, WI ",
                               Val = "55097",
                           },
                           new GeoFips
                           {
                               Description = "4 Price, WI ",
                               Val = "55099",
                           },
                           new GeoFips
                           {
                               Description = "4 Racine, WI ",
                               Val = "55101",
                           },
                           new GeoFips
                           {
                               Description = "4 Richland, WI ",
                               Val = "55103",
                           },
                           new GeoFips
                           {
                               Description = "4 Rock, WI ",
                               Val = "55105",
                           },
                           new GeoFips
                           {
                               Description = "4 Rusk, WI ",
                               Val = "55107",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Croix, WI ",
                               Val = "55109",
                           },
                           new GeoFips
                           {
                               Description = "4 Sauk, WI ",
                               Val = "55111",
                           },
                           new GeoFips
                           {
                               Description = "4 Sawyer, WI ",
                               Val = "55113",
                           },
                           new GeoFips
                           {
                               Description = "4 Shawano, WI ",
                               Val = "55115",
                           },
                           new GeoFips
                           {
                               Description = "4 Sheboygan, WI ",
                               Val = "55117",
                           },
                           new GeoFips
                           {
                               Description = "4 Taylor, WI ",
                               Val = "55119",
                           },
                           new GeoFips
                           {
                               Description = "4 Trempealeau, WI ",
                               Val = "55121",
                           },
                           new GeoFips
                           {
                               Description = "4 Vernon, WI ",
                               Val = "55123",
                           },
                           new GeoFips
                           {
                               Description = "4 Vilas, WI ",
                               Val = "55125",
                           },
                           new GeoFips
                           {
                               Description = "4 Walworth, WI ",
                               Val = "55127",
                           },
                           new GeoFips
                           {
                               Description = "4 Washburn, WI ",
                               Val = "55129",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, WI ",
                               Val = "55131",
                           },
                           new GeoFips
                           {
                               Description = "4 Waukesha, WI ",
                               Val = "55133",
                           },
                           new GeoFips
                           {
                               Description = "4 Waupaca, WI ",
                               Val = "55135",
                           },
                           new GeoFips
                           {
                               Description = "4 Waushara, WI ",
                               Val = "55137",
                           },
                           new GeoFips
                           {
                               Description = "4 Winnebago, WI ",
                               Val = "55139",
                           },
                           new GeoFips
                           {
                               Description = "4 Wood, WI ",
                               Val = "55141",
                           },
                           new GeoFips
                           {
                               Description = "4 Shawano (includes Menominee), WI ",
                               Val = "55901",
                           },
                           new GeoFips
                           {
                               Description = "3 Wyoming",
                               Val = "56000",
                           },
                           new GeoFips
                           {
                               Description = "4 Albany, WY ",
                               Val = "56001",
                           },
                           new GeoFips
                           {
                               Description = "4 Big Horn, WY ",
                               Val = "56003",
                           },
                           new GeoFips
                           {
                               Description = "4 Campbell, WY ",
                               Val = "56005",
                           },
                           new GeoFips
                           {
                               Description = "4 Carbon, WY ",
                               Val = "56007",
                           },
                           new GeoFips
                           {
                               Description = "4 Converse, WY ",
                               Val = "56009",
                           },
                           new GeoFips
                           {
                               Description = "4 Crook, WY ",
                               Val = "56011",
                           },
                           new GeoFips
                           {
                               Description = "4 Fremont, WY ",
                               Val = "56013",
                           },
                           new GeoFips
                           {
                               Description = "4 Goshen, WY ",
                               Val = "56015",
                           },
                           new GeoFips
                           {
                               Description = "4 Hot Springs, WY ",
                               Val = "56017",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, WY ",
                               Val = "56019",
                           },
                           new GeoFips
                           {
                               Description = "4 Laramie, WY ",
                               Val = "56021",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, WY ",
                               Val = "56023",
                           },
                           new GeoFips
                           {
                               Description = "4 Natrona, WY ",
                               Val = "56025",
                           },
                           new GeoFips
                           {
                               Description = "4 Niobrara, WY ",
                               Val = "56027",
                           },
                           new GeoFips
                           {
                               Description = "4 Park, WY ",
                               Val = "56029",
                           },
                           new GeoFips
                           {
                               Description = "4 Platte, WY ",
                               Val = "56031",
                           },
                           new GeoFips
                           {
                               Description = "4 Sheridan, WY ",
                               Val = "56033",
                           },
                           new GeoFips
                           {
                               Description = "4 Sublette, WY ",
                               Val = "56035",
                           },
                           new GeoFips
                           {
                               Description = "4 Sweetwater, WY ",
                               Val = "56037",
                           },
                           new GeoFips
                           {
                               Description = "4 Teton, WY ",
                               Val = "56039",
                           },
                           new GeoFips
                           {
                               Description = "4 Uinta, WY ",
                               Val = "56041",
                           },
                           new GeoFips
                           {
                               Description = "4 Washakie, WY ",
                               Val = "56043",
                           },
                           new GeoFips
                           {
                               Description = "4 Weston, WY ",
                               Val = "56045",
                           },
                           new GeoFips
                           {
                               Description = "2 New England",
                               Val = "91000",
                           },
                           new GeoFips
                           {
                               Description = "2 Mideast",
                               Val = "92000",
                           },
                           new GeoFips
                           {
                               Description = "2 Great Lakes",
                               Val = "93000",
                           },
                           new GeoFips
                           {
                               Description = "2 Plains",
                               Val = "94000",
                           },
                           new GeoFips
                           {
                               Description = "2 Southeast",
                               Val = "95000",
                           },
                           new GeoFips
                           {
                               Description = "2 Southwest",
                               Val = "96000",
                           },
                           new GeoFips
                           {
                               Description = "2 Rocky Mountain",
                               Val = "97000",
                           },
                           new GeoFips
                           {
                               Description = "2 Far West",
                               Val = "98000",
                           },
                           new GeoFips
                           {
                               Description = "1 United States",
                               Val = "00000",
                           },
                           new GeoFips
                           {
                               Description = "12 United States (Metropolitan Portion)",
                               Val = "00998",
                           },
                           new GeoFips
                           {
                               Description = "13 United States (Nonmetropolitan Portion)",
                               Val = "00999",
                           },
                           new GeoFips
                           {
                               Description = "3 Alabama",
                               Val = "01000",
                           },
                           new GeoFips
                           {
                               Description = "4 Autauga, AL ",
                               Val = "01001",
                           },
                           new GeoFips
                           {
                               Description = "4 Baldwin, AL ",
                               Val = "01003",
                           },
                           new GeoFips
                           {
                               Description = "4 Barbour, AL ",
                               Val = "01005",
                           },
                           new GeoFips
                           {
                               Description = "4 Bibb, AL ",
                               Val = "01007",
                           },
                           new GeoFips
                           {
                               Description = "4 Blount, AL ",
                               Val = "01009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bullock, AL ",
                               Val = "01011",
                           },
                           new GeoFips
                           {
                               Description = "4 Butler, AL ",
                               Val = "01013",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, AL ",
                               Val = "01015",
                           },
                           new GeoFips
                           {
                               Description = "4 Chambers, AL ",
                               Val = "01017",
                           },
                           new GeoFips
                           {
                               Description = "4 Cherokee, AL ",
                               Val = "01019",
                           },
                           new GeoFips
                           {
                               Description = "4 Chilton, AL ",
                               Val = "01021",
                           },
                           new GeoFips
                           {
                               Description = "4 Choctaw, AL ",
                               Val = "01023",
                           },
                           new GeoFips
                           {
                               Description = "4 Clarke, AL ",
                               Val = "01025",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, AL ",
                               Val = "01027",
                           },
                           new GeoFips
                           {
                               Description = "4 Cleburne, AL ",
                               Val = "01029",
                           },
                           new GeoFips
                           {
                               Description = "4 Coffee, AL ",
                               Val = "01031",
                           },
                           new GeoFips
                           {
                               Description = "4 Colbert, AL ",
                               Val = "01033",
                           },
                           new GeoFips
                           {
                               Description = "4 Conecuh, AL ",
                               Val = "01035",
                           },
                           new GeoFips
                           {
                               Description = "4 Coosa, AL ",
                               Val = "01037",
                           },
                           new GeoFips
                           {
                               Description = "4 Covington, AL ",
                               Val = "01039",
                           },
                           new GeoFips
                           {
                               Description = "4 Crenshaw, AL ",
                               Val = "01041",
                           },
                           new GeoFips
                           {
                               Description = "4 Cullman, AL ",
                               Val = "01043",
                           },
                           new GeoFips
                           {
                               Description = "4 Dale, AL ",
                               Val = "01045",
                           },
                           new GeoFips
                           {
                               Description = "4 Dallas, AL ",
                               Val = "01047",
                           },
                           new GeoFips
                           {
                               Description = "4 DeKalb, AL ",
                               Val = "01049",
                           },
                           new GeoFips
                           {
                               Description = "4 Elmore, AL ",
                               Val = "01051",
                           },
                           new GeoFips
                           {
                               Description = "4 Escambia, AL ",
                               Val = "01053",
                           },
                           new GeoFips
                           {
                               Description = "4 Etowah, AL ",
                               Val = "01055",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, AL ",
                               Val = "01057",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, AL ",
                               Val = "01059",
                           },
                           new GeoFips
                           {
                               Description = "4 Geneva, AL ",
                               Val = "01061",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, AL ",
                               Val = "01063",
                           },
                           new GeoFips
                           {
                               Description = "4 Hale, AL ",
                               Val = "01065",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry, AL ",
                               Val = "01067",
                           },
                           new GeoFips
                           {
                               Description = "4 Houston, AL ",
                               Val = "01069",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, AL ",
                               Val = "01071",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, AL ",
                               Val = "01073",
                           },
                           new GeoFips
                           {
                               Description = "4 Lamar, AL ",
                               Val = "01075",
                           },
                           new GeoFips
                           {
                               Description = "4 Lauderdale, AL ",
                               Val = "01077",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, AL ",
                               Val = "01079",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, AL ",
                               Val = "01081",
                           },
                           new GeoFips
                           {
                               Description = "4 Limestone, AL ",
                               Val = "01083",
                           },
                           new GeoFips
                           {
                               Description = "4 Lowndes, AL ",
                               Val = "01085",
                           },
                           new GeoFips
                           {
                               Description = "4 Macon, AL ",
                               Val = "01087",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, AL ",
                               Val = "01089",
                           },
                           new GeoFips
                           {
                               Description = "4 Marengo, AL ",
                               Val = "01091",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, AL ",
                               Val = "01093",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, AL ",
                               Val = "01095",
                           },
                           new GeoFips
                           {
                               Description = "4 Mobile, AL ",
                               Val = "01097",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, AL ",
                               Val = "01099",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, AL ",
                               Val = "01101",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, AL ",
                               Val = "01103",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, AL ",
                               Val = "01105",
                           },
                           new GeoFips
                           {
                               Description = "4 Pickens, AL ",
                               Val = "01107",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, AL ",
                               Val = "01109",
                           },
                           new GeoFips
                           {
                               Description = "4 Randolph, AL ",
                               Val = "01111",
                           },
                           new GeoFips
                           {
                               Description = "4 Russell, AL ",
                               Val = "01113",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Clair, AL ",
                               Val = "01115",
                           },
                           new GeoFips
                           {
                               Description = "4 Shelby, AL ",
                               Val = "01117",
                           },
                           new GeoFips
                           {
                               Description = "4 Sumter, AL ",
                               Val = "01119",
                           },
                           new GeoFips
                           {
                               Description = "4 Talladega, AL ",
                               Val = "01121",
                           },
                           new GeoFips
                           {
                               Description = "4 Tallapoosa, AL ",
                               Val = "01123",
                           },
                           new GeoFips
                           {
                               Description = "4 Tuscaloosa, AL ",
                               Val = "01125",
                           },
                           new GeoFips
                           {
                               Description = "4 Walker, AL ",
                               Val = "01127",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, AL ",
                               Val = "01129",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilcox, AL ",
                               Val = "01131",
                           },
                           new GeoFips
                           {
                               Description = "4 Winston, AL ",
                               Val = "01133",
                           },
                           new GeoFips
                           {
                               Description = "3 Alaska",
                               Val = "02000",
                           },
                           new GeoFips
                           {
                               Description = "4 Aleutian Islands Census Area, AK ",
                               Val = "02010",
                           },
                           new GeoFips
                           {
                               Description = "4 Aleutians East Borough, AK ",
                               Val = "02013",
                           },
                           new GeoFips
                           {
                               Description = "4 Aleutians West Census Area, AK ",
                               Val = "02016",
                           },
                           new GeoFips
                           {
                               Description = "4 Anchorage Municipality, AK ",
                               Val = "02020",
                           },
                           new GeoFips
                           {
                               Description = "4 Bethel Census Area, AK ",
                               Val = "02050",
                           },
                           new GeoFips
                           {
                               Description = "4 Bristol Bay Borough, AK ",
                               Val = "02060",
                           },
                           new GeoFips
                           {
                               Description = "4 Denali Borough, AK ",
                               Val = "02068",
                           },
                           new GeoFips
                           {
                               Description = "4 Dillingham Census Area, AK ",
                               Val = "02070",
                           },
                           new GeoFips
                           {
                               Description = "4 Fairbanks North Star Borough, AK ",
                               Val = "02090",
                           },
                           new GeoFips
                           {
                               Description = "4 Haines Borough, AK ",
                               Val = "02100",
                           },
                           new GeoFips
                           {
                               Description = "4 Hoonah-Angoon Census Area, AK ",
                               Val = "02105",
                           },
                           new GeoFips
                           {
                               Description = "4 Juneau City and Borough, AK ",
                               Val = "02110",
                           },
                           new GeoFips
                           {
                               Description = "4 Kenai Peninsula Borough, AK ",
                               Val = "02122",
                           },
                           new GeoFips
                           {
                               Description = "4 Ketchikan Gateway Borough, AK ",
                               Val = "02130",
                           },
                           new GeoFips
                           {
                               Description = "4 Kodiak Island Borough, AK ",
                               Val = "02150",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake and Peninsula Borough, AK ",
                               Val = "02164",
                           },
                           new GeoFips
                           {
                               Description = "4 Matanuska-Susitna Borough, AK ",
                               Val = "02170",
                           },
                           new GeoFips
                           {
                               Description = "4 Nome Census Area, AK ",
                               Val = "02180",
                           },
                           new GeoFips
                           {
                               Description = "4 North Slope Borough, AK ",
                               Val = "02185",
                           },
                           new GeoFips
                           {
                               Description = "4 Northwest Arctic Borough, AK ",
                               Val = "02188",
                           },
                           new GeoFips
                           {
                               Description = "4 Petersburg Census Area, AK ",
                               Val = "02195",
                           },
                           new GeoFips
                           {
                               Description = "4 Prince of Wales-Hyder Census Area, AK ",
                               Val = "02198",
                           },
                           new GeoFips
                           {
                               Description = "4 Prince of Wales-Outer Ketchikan Census Area, AK ",
                               Val = "02201",
                           },
                           new GeoFips
                           {
                               Description = "4 Sitka City and Borough, AK ",
                               Val = "02220",
                           },
                           new GeoFips
                           {
                               Description = "4 Skagway Municipality, AK ",
                               Val = "02230",
                           },
                           new GeoFips
                           {
                               Description = "4 Skagway-Yakutat-Angoon Census Area, AK ",
                               Val = "02231",
                           },
                           new GeoFips
                           {
                               Description = "4 Skagway-Hoonah-Angoon Census Area, AK ",
                               Val = "02232",
                           },
                           new GeoFips
                           {
                               Description = "4 Southeast Fairbanks Census Area, AK ",
                               Val = "02240",
                           },
                           new GeoFips
                           {
                               Description = "4 Valdez-Cordova Census Area, AK ",
                               Val = "02261",
                           },
                           new GeoFips
                           {
                               Description = "4 Wade Hampton Census Area, AK ",
                               Val = "02270",
                           },
                           new GeoFips
                           {
                               Description = "4 Wrangell City and Borough, AK ",
                               Val = "02275",
                           },
                           new GeoFips
                           {
                               Description = "4 Wrangell-Petersburg Census Area, AK ",
                               Val = "02280",
                           },
                           new GeoFips
                           {
                               Description = "4 Yakutat City and Borough, AK ",
                               Val = "02282",
                           },
                           new GeoFips
                           {
                               Description = "4 Yukon-Koyukuk Census Area, AK ",
                               Val = "02290",
                           },
                           new GeoFips
                           {
                               Description = "4 Aleutian Islands Division, AK ",
                               Val = "02901",
                           },
                           new GeoFips
                           {
                               Description = "4 Angoon Division, AK ",
                               Val = "02903",
                           },
                           new GeoFips
                           {
                               Description = "4 Barrow-North Slope Division, AK ",
                               Val = "02904",
                           },
                           new GeoFips
                           {
                               Description = "4 Bethel Division, AK ",
                               Val = "02905",
                           },
                           new GeoFips
                           {
                               Description = "4 Bristol Bay Division, AK ",
                               Val = "02907",
                           },
                           new GeoFips
                           {
                               Description = "4 Cordova-McCarthy Division, AK ",
                               Val = "02908",
                           },
                           new GeoFips
                           {
                               Description = "4 Haines Division, AK ",
                               Val = "02910",
                           },
                           new GeoFips
                           {
                               Description = "4 Kenai-Cook Inlet Division, AK ",
                               Val = "02912",
                           },
                           new GeoFips
                           {
                               Description = "4 Kuskokwim Division, AK ",
                               Val = "02916",
                           },
                           new GeoFips
                           {
                               Description = "4 Outer Ketchikan Division, AK ",
                               Val = "02919",
                           },
                           new GeoFips
                           {
                               Description = "4 Prince of Wales Division, AK ",
                               Val = "02920",
                           },
                           new GeoFips
                           {
                               Description = "4 Seward Division, AK ",
                               Val = "02921",
                           },
                           new GeoFips
                           {
                               Description = "4 Sitka Division, AK ",
                               Val = "02922",
                           },
                           new GeoFips
                           {
                               Description = "4 Skagway-Yakutat Division, AK ",
                               Val = "02923",
                           },
                           new GeoFips
                           {
                               Description = "4 Southeast Fairbanks Division, AK ",
                               Val = "02924",
                           },
                           new GeoFips
                           {
                               Description = "4 Upper Yukon Division, AK ",
                               Val = "02925",
                           },
                           new GeoFips
                           {
                               Description = "4 Valdez-Chitina-Whittier Division, AK ",
                               Val = "02926",
                           },
                           new GeoFips
                           {
                               Description = "4 Wrangell-Petersburg Division, AK ",
                               Val = "02928",
                           },
                           new GeoFips
                           {
                               Description = "4 Yukon-Koyukuk Division, AK ",
                               Val = "02929",
                           },
                           new GeoFips
                           {
                               Description = "3 Arizona",
                               Val = "04000",
                           },
                           new GeoFips
                           {
                               Description = "4 Apache, AZ ",
                               Val = "04001",
                           },
                           new GeoFips
                           {
                               Description = "4 Cochise, AZ ",
                               Val = "04003",
                           },
                           new GeoFips
                           {
                               Description = "4 Coconino, AZ ",
                               Val = "04005",
                           },
                           new GeoFips
                           {
                               Description = "4 Gila, AZ ",
                               Val = "04007",
                           },
                           new GeoFips
                           {
                               Description = "4 Graham, AZ ",
                               Val = "04009",
                           },
                           new GeoFips
                           {
                               Description = "4 Greenlee, AZ ",
                               Val = "04011",
                           },
                           new GeoFips
                           {
                               Description = "4 La Paz, AZ ",
                               Val = "04012",
                           },
                           new GeoFips
                           {
                               Description = "4 Maricopa, AZ ",
                               Val = "04013",
                           },
                           new GeoFips
                           {
                               Description = "4 Mohave, AZ ",
                               Val = "04015",
                           },
                           new GeoFips
                           {
                               Description = "4 Navajo, AZ ",
                               Val = "04017",
                           },
                           new GeoFips
                           {
                               Description = "4 Pima, AZ ",
                               Val = "04019",
                           },
                           new GeoFips
                           {
                               Description = "4 Pinal, AZ ",
                               Val = "04021",
                           },
                           new GeoFips
                           {
                               Description = "4 Santa Cruz, AZ ",
                               Val = "04023",
                           },
                           new GeoFips
                           {
                               Description = "4 Yavapai, AZ ",
                               Val = "04025",
                           },
                           new GeoFips
                           {
                               Description = "4 Yuma, AZ ",
                               Val = "04027",
                           },
                           new GeoFips
                           {
                               Description = "3 Arkansas",
                               Val = "05000",
                           },
                           new GeoFips
                           {
                               Description = "4 Arkansas, AR ",
                               Val = "05001",
                           },
                           new GeoFips
                           {
                               Description = "4 Ashley, AR ",
                               Val = "05003",
                           },
                           new GeoFips
                           {
                               Description = "4 Baxter, AR ",
                               Val = "05005",
                           },
                           new GeoFips
                           {
                               Description = "4 Benton, AR ",
                               Val = "05007",
                           },
                           new GeoFips
                           {
                               Description = "4 Boone, AR ",
                               Val = "05009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bradley, AR ",
                               Val = "05011",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, AR ",
                               Val = "05013",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, AR ",
                               Val = "05015",
                           },
                           new GeoFips
                           {
                               Description = "4 Chicot, AR ",
                               Val = "05017",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, AR ",
                               Val = "05019",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, AR ",
                               Val = "05021",
                           },
                           new GeoFips
                           {
                               Description = "4 Cleburne, AR ",
                               Val = "05023",
                           },
                           new GeoFips
                           {
                               Description = "4 Cleveland, AR ",
                               Val = "05025",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbia, AR ",
                               Val = "05027",
                           },
                           new GeoFips
                           {
                               Description = "4 Conway, AR ",
                               Val = "05029",
                           },
                           new GeoFips
                           {
                               Description = "4 Craighead, AR ",
                               Val = "05031",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, AR ",
                               Val = "05033",
                           },
                           new GeoFips
                           {
                               Description = "4 Crittenden, AR ",
                               Val = "05035",
                           },
                           new GeoFips
                           {
                               Description = "4 Cross, AR ",
                               Val = "05037",
                           },
                           new GeoFips
                           {
                               Description = "4 Dallas, AR ",
                               Val = "05039",
                           },
                           new GeoFips
                           {
                               Description = "4 Desha, AR ",
                               Val = "05041",
                           },
                           new GeoFips
                           {
                               Description = "4 Drew, AR ",
                               Val = "05043",
                           },
                           new GeoFips
                           {
                               Description = "4 Faulkner, AR ",
                               Val = "05045",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, AR ",
                               Val = "05047",
                           },
                           new GeoFips
                           {
                               Description = "4 Fulton, AR ",
                               Val = "05049",
                           },
                           new GeoFips
                           {
                               Description = "4 Garland, AR ",
                               Val = "05051",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, AR ",
                               Val = "05053",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, AR ",
                               Val = "05055",
                           },
                           new GeoFips
                           {
                               Description = "4 Hempstead, AR ",
                               Val = "05057",
                           },
                           new GeoFips
                           {
                               Description = "4 Hot Spring, AR ",
                               Val = "05059",
                           },
                           new GeoFips
                           {
                               Description = "4 Howard, AR ",
                               Val = "05061",
                           },
                           new GeoFips
                           {
                               Description = "4 Independence, AR ",
                               Val = "05063",
                           },
                           new GeoFips
                           {
                               Description = "4 Izard, AR ",
                               Val = "05065",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, AR ",
                               Val = "05067",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, AR ",
                               Val = "05069",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, AR ",
                               Val = "05071",
                           },
                           new GeoFips
                           {
                               Description = "4 Lafayette, AR ",
                               Val = "05073",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, AR ",
                               Val = "05075",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, AR ",
                               Val = "05077",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, AR ",
                               Val = "05079",
                           },
                           new GeoFips
                           {
                               Description = "4 Little River, AR ",
                               Val = "05081",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, AR ",
                               Val = "05083",
                           },
                           new GeoFips
                           {
                               Description = "4 Lonoke, AR ",
                               Val = "05085",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, AR ",
                               Val = "05087",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, AR ",
                               Val = "05089",
                           },
                           new GeoFips
                           {
                               Description = "4 Miller, AR ",
                               Val = "05091",
                           },
                           new GeoFips
                           {
                               Description = "4 Mississippi, AR ",
                               Val = "05093",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, AR ",
                               Val = "05095",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, AR ",
                               Val = "05097",
                           },
                           new GeoFips
                           {
                               Description = "4 Nevada, AR ",
                               Val = "05099",
                           },
                           new GeoFips
                           {
                               Description = "4 Newton, AR ",
                               Val = "05101",
                           },
                           new GeoFips
                           {
                               Description = "4 Ouachita, AR ",
                               Val = "05103",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, AR ",
                               Val = "05105",
                           },
                           new GeoFips
                           {
                               Description = "4 Phillips, AR ",
                               Val = "05107",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, AR ",
                               Val = "05109",
                           },
                           new GeoFips
                           {
                               Description = "4 Poinsett, AR ",
                               Val = "05111",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, AR ",
                               Val = "05113",
                           },
                           new GeoFips
                           {
                               Description = "4 Pope, AR ",
                               Val = "05115",
                           },
                           new GeoFips
                           {
                               Description = "4 Prairie, AR ",
                               Val = "05117",
                           },
                           new GeoFips
                           {
                               Description = "4 Pulaski, AR ",
                               Val = "05119",
                           },
                           new GeoFips
                           {
                               Description = "4 Randolph, AR ",
                               Val = "05121",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Francis, AR ",
                               Val = "05123",
                           },
                           new GeoFips
                           {
                               Description = "4 Saline, AR ",
                               Val = "05125",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, AR ",
                               Val = "05127",
                           },
                           new GeoFips
                           {
                               Description = "4 Searcy, AR ",
                               Val = "05129",
                           },
                           new GeoFips
                           {
                               Description = "4 Sebastian, AR ",
                               Val = "05131",
                           },
                           new GeoFips
                           {
                               Description = "4 Sevier, AR ",
                               Val = "05133",
                           },
                           new GeoFips
                           {
                               Description = "4 Sharp, AR ",
                               Val = "05135",
                           },
                           new GeoFips
                           {
                               Description = "4 Stone, AR ",
                               Val = "05137",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, AR ",
                               Val = "05139",
                           },
                           new GeoFips
                           {
                               Description = "4 Van Buren, AR ",
                               Val = "05141",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, AR ",
                               Val = "05143",
                           },
                           new GeoFips
                           {
                               Description = "4 White, AR ",
                               Val = "05145",
                           },
                           new GeoFips
                           {
                               Description = "4 Woodruff, AR ",
                               Val = "05147",
                           },
                           new GeoFips
                           {
                               Description = "4 Yell, AR ",
                               Val = "05149",
                           },
                           new GeoFips
                           {
                               Description = "3 California",
                               Val = "06000",
                           },
                           new GeoFips
                           {
                               Description = "4 Alameda, CA ",
                               Val = "06001",
                           },
                           new GeoFips
                           {
                               Description = "4 Alpine, CA ",
                               Val = "06003",
                           },
                           new GeoFips
                           {
                               Description = "4 Amador, CA ",
                               Val = "06005",
                           },
                           new GeoFips
                           {
                               Description = "4 Butte, CA ",
                               Val = "06007",
                           },
                           new GeoFips
                           {
                               Description = "4 Calaveras, CA ",
                               Val = "06009",
                           },
                           new GeoFips
                           {
                               Description = "4 Colusa, CA ",
                               Val = "06011",
                           },
                           new GeoFips
                           {
                               Description = "4 Contra Costa, CA ",
                               Val = "06013",
                           },
                           new GeoFips
                           {
                               Description = "4 Del Norte, CA ",
                               Val = "06015",
                           },
                           new GeoFips
                           {
                               Description = "4 El Dorado, CA ",
                               Val = "06017",
                           },
                           new GeoFips
                           {
                               Description = "4 Fresno, CA ",
                               Val = "06019",
                           },
                           new GeoFips
                           {
                               Description = "4 Glenn, CA ",
                               Val = "06021",
                           },
                           new GeoFips
                           {
                               Description = "4 Humboldt, CA ",
                               Val = "06023",
                           },
                           new GeoFips
                           {
                               Description = "4 Imperial, CA ",
                               Val = "06025",
                           },
                           new GeoFips
                           {
                               Description = "4 Inyo, CA ",
                               Val = "06027",
                           },
                           new GeoFips
                           {
                               Description = "4 Kern, CA ",
                               Val = "06029",
                           },
                           new GeoFips
                           {
                               Description = "4 Kings, CA ",
                               Val = "06031",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, CA ",
                               Val = "06033",
                           },
                           new GeoFips
                           {
                               Description = "4 Lassen, CA ",
                               Val = "06035",
                           },
                           new GeoFips
                           {
                               Description = "4 Los Angeles, CA ",
                               Val = "06037",
                           },
                           new GeoFips
                           {
                               Description = "4 Madera, CA ",
                               Val = "06039",
                           },
                           new GeoFips
                           {
                               Description = "4 Marin, CA ",
                               Val = "06041",
                           },
                           new GeoFips
                           {
                               Description = "4 Mariposa, CA ",
                               Val = "06043",
                           },
                           new GeoFips
                           {
                               Description = "4 Mendocino, CA ",
                               Val = "06045",
                           },
                           new GeoFips
                           {
                               Description = "4 Merced, CA ",
                               Val = "06047",
                           },
                           new GeoFips
                           {
                               Description = "4 Modoc, CA ",
                               Val = "06049",
                           },
                           new GeoFips
                           {
                               Description = "4 Mono, CA ",
                               Val = "06051",
                           },
                           new GeoFips
                           {
                               Description = "4 Monterey, CA ",
                               Val = "06053",
                           },
                           new GeoFips
                           {
                               Description = "4 Napa, CA ",
                               Val = "06055",
                           },
                           new GeoFips
                           {
                               Description = "4 Nevada, CA ",
                               Val = "06057",
                           },
                           new GeoFips
                           {
                               Description = "4 Orange, CA ",
                               Val = "06059",
                           },
                           new GeoFips
                           {
                               Description = "4 Placer, CA ",
                               Val = "06061",
                           },
                           new GeoFips
                           {
                               Description = "4 Plumas, CA ",
                               Val = "06063",
                           },
                           new GeoFips
                           {
                               Description = "4 Riverside, CA ",
                               Val = "06065",
                           },
                           new GeoFips
                           {
                               Description = "4 Sacramento, CA ",
                               Val = "06067",
                           },
                           new GeoFips
                           {
                               Description = "4 San Benito, CA ",
                               Val = "06069",
                           },
                           new GeoFips
                           {
                               Description = "4 San Bernardino, CA ",
                               Val = "06071",
                           },
                           new GeoFips
                           {
                               Description = "4 San Diego, CA ",
                               Val = "06073",
                           },
                           new GeoFips
                           {
                               Description = "4 San Francisco, CA ",
                               Val = "06075",
                           },
                           new GeoFips
                           {
                               Description = "4 San Joaquin, CA ",
                               Val = "06077",
                           },
                           new GeoFips
                           {
                               Description = "4 San Luis Obispo, CA ",
                               Val = "06079",
                           },
                           new GeoFips
                           {
                               Description = "4 San Mateo, CA ",
                               Val = "06081",
                           },
                           new GeoFips
                           {
                               Description = "4 Santa Barbara, CA ",
                               Val = "06083",
                           },
                           new GeoFips
                           {
                               Description = "4 Santa Clara, CA ",
                               Val = "06085",
                           },
                           new GeoFips
                           {
                               Description = "4 Santa Cruz, CA ",
                               Val = "06087",
                           },
                           new GeoFips
                           {
                               Description = "4 Shasta, CA ",
                               Val = "06089",
                           },
                           new GeoFips
                           {
                               Description = "4 Sierra, CA ",
                               Val = "06091",
                           },
                           new GeoFips
                           {
                               Description = "4 Siskiyou, CA ",
                               Val = "06093",
                           },
                           new GeoFips
                           {
                               Description = "4 Solano, CA ",
                               Val = "06095",
                           },
                           new GeoFips
                           {
                               Description = "4 Sonoma, CA ",
                               Val = "06097",
                           },
                           new GeoFips
                           {
                               Description = "4 Stanislaus, CA ",
                               Val = "06099",
                           },
                           new GeoFips
                           {
                               Description = "4 Sutter, CA ",
                               Val = "06101",
                           },
                           new GeoFips
                           {
                               Description = "4 Tehama, CA ",
                               Val = "06103",
                           },
                           new GeoFips
                           {
                               Description = "4 Trinity, CA ",
                               Val = "06105",
                           },
                           new GeoFips
                           {
                               Description = "4 Tulare, CA ",
                               Val = "06107",
                           },
                           new GeoFips
                           {
                               Description = "4 Tuolumne, CA ",
                               Val = "06109",
                           },
                           new GeoFips
                           {
                               Description = "4 Ventura, CA ",
                               Val = "06111",
                           },
                           new GeoFips
                           {
                               Description = "4 Yolo, CA ",
                               Val = "06113",
                           },
                           new GeoFips
                           {
                               Description = "4 Yuba, CA ",
                               Val = "06115",
                           },
                           new GeoFips
                           {
                               Description = "3 Colorado",
                               Val = "08000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, CO ",
                               Val = "08001",
                           },
                           new GeoFips
                           {
                               Description = "4 Alamosa, CO ",
                               Val = "08003",
                           },
                           new GeoFips
                           {
                               Description = "4 Arapahoe, CO ",
                               Val = "08005",
                           },
                           new GeoFips
                           {
                               Description = "4 Archuleta, CO ",
                               Val = "08007",
                           },
                           new GeoFips
                           {
                               Description = "4 Baca, CO ",
                               Val = "08009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bent, CO ",
                               Val = "08011",
                           },
                           new GeoFips
                           {
                               Description = "4 Boulder, CO ",
                               Val = "08013",
                           },
                           new GeoFips
                           {
                               Description = "4 Broomfield, CO ",
                               Val = "08014",
                           },
                           new GeoFips
                           {
                               Description = "4 Chaffee, CO ",
                               Val = "08015",
                           },
                           new GeoFips
                           {
                               Description = "4 Cheyenne, CO ",
                               Val = "08017",
                           },
                           new GeoFips
                           {
                               Description = "4 Clear Creek, CO ",
                               Val = "08019",
                           },
                           new GeoFips
                           {
                               Description = "4 Conejos, CO ",
                               Val = "08021",
                           },
                           new GeoFips
                           {
                               Description = "4 Costilla, CO ",
                               Val = "08023",
                           },
                           new GeoFips
                           {
                               Description = "4 Crowley, CO ",
                               Val = "08025",
                           },
                           new GeoFips
                           {
                               Description = "4 Custer, CO ",
                               Val = "08027",
                           },
                           new GeoFips
                           {
                               Description = "4 Delta, CO ",
                               Val = "08029",
                           },
                           new GeoFips
                           {
                               Description = "4 Denver, CO ",
                               Val = "08031",
                           },
                           new GeoFips
                           {
                               Description = "4 Dolores, CO ",
                               Val = "08033",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, CO ",
                               Val = "08035",
                           },
                           new GeoFips
                           {
                               Description = "4 Eagle, CO ",
                               Val = "08037",
                           },
                           new GeoFips
                           {
                               Description = "4 Elbert, CO ",
                               Val = "08039",
                           },
                           new GeoFips
                           {
                               Description = "4 El Paso, CO ",
                               Val = "08041",
                           },
                           new GeoFips
                           {
                               Description = "4 Fremont, CO ",
                               Val = "08043",
                           },
                           new GeoFips
                           {
                               Description = "4 Garfield, CO ",
                               Val = "08045",
                           },
                           new GeoFips
                           {
                               Description = "4 Gilpin, CO ",
                               Val = "08047",
                           },
                           new GeoFips
                           {
                               Description = "4 Grand, CO ",
                               Val = "08049",
                           },
                           new GeoFips
                           {
                               Description = "4 Gunnison, CO ",
                               Val = "08051",
                           },
                           new GeoFips
                           {
                               Description = "4 Hinsdale, CO ",
                               Val = "08053",
                           },
                           new GeoFips
                           {
                               Description = "4 Huerfano, CO ",
                               Val = "08055",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, CO ",
                               Val = "08057",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, CO ",
                               Val = "08059",
                           },
                           new GeoFips
                           {
                               Description = "4 Kiowa, CO ",
                               Val = "08061",
                           },
                           new GeoFips
                           {
                               Description = "4 Kit Carson, CO ",
                               Val = "08063",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, CO ",
                               Val = "08065",
                           },
                           new GeoFips
                           {
                               Description = "4 La Plata, CO ",
                               Val = "08067",
                           },
                           new GeoFips
                           {
                               Description = "4 Larimer, CO ",
                               Val = "08069",
                           },
                           new GeoFips
                           {
                               Description = "4 Las Animas, CO ",
                               Val = "08071",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, CO ",
                               Val = "08073",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, CO ",
                               Val = "08075",
                           },
                           new GeoFips
                           {
                               Description = "4 Mesa, CO ",
                               Val = "08077",
                           },
                           new GeoFips
                           {
                               Description = "4 Mineral, CO ",
                               Val = "08079",
                           },
                           new GeoFips
                           {
                               Description = "4 Moffat, CO ",
                               Val = "08081",
                           },
                           new GeoFips
                           {
                               Description = "4 Montezuma, CO ",
                               Val = "08083",
                           },
                           new GeoFips
                           {
                               Description = "4 Montrose, CO ",
                               Val = "08085",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, CO ",
                               Val = "08087",
                           },
                           new GeoFips
                           {
                               Description = "4 Otero, CO ",
                               Val = "08089",
                           },
                           new GeoFips
                           {
                               Description = "4 Ouray, CO ",
                               Val = "08091",
                           },
                           new GeoFips
                           {
                               Description = "4 Park, CO ",
                               Val = "08093",
                           },
                           new GeoFips
                           {
                               Description = "4 Phillips, CO ",
                               Val = "08095",
                           },
                           new GeoFips
                           {
                               Description = "4 Pitkin, CO ",
                               Val = "08097",
                           },
                           new GeoFips
                           {
                               Description = "4 Prowers, CO ",
                               Val = "08099",
                           },
                           new GeoFips
                           {
                               Description = "4 Pueblo, CO ",
                               Val = "08101",
                           },
                           new GeoFips
                           {
                               Description = "4 Rio Blanco, CO ",
                               Val = "08103",
                           },
                           new GeoFips
                           {
                               Description = "4 Rio Grande, CO ",
                               Val = "08105",
                           },
                           new GeoFips
                           {
                               Description = "4 Routt, CO ",
                               Val = "08107",
                           },
                           new GeoFips
                           {
                               Description = "4 Saguache, CO ",
                               Val = "08109",
                           },
                           new GeoFips
                           {
                               Description = "4 San Juan, CO ",
                               Val = "08111",
                           },
                           new GeoFips
                           {
                               Description = "4 San Miguel, CO ",
                               Val = "08113",
                           },
                           new GeoFips
                           {
                               Description = "4 Sedgwick, CO ",
                               Val = "08115",
                           },
                           new GeoFips
                           {
                               Description = "4 Summit, CO ",
                               Val = "08117",
                           },
                           new GeoFips
                           {
                               Description = "4 Teller, CO ",
                               Val = "08119",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, CO ",
                               Val = "08121",
                           },
                           new GeoFips
                           {
                               Description = "4 Weld, CO ",
                               Val = "08123",
                           },
                           new GeoFips
                           {
                               Description = "4 Yuma, CO ",
                               Val = "08125",
                           },
                           new GeoFips
                           {
                               Description = "3 Connecticut",
                               Val = "09000",
                           },
                           new GeoFips
                           {
                               Description = "4 Fairfield, CT ",
                               Val = "09001",
                           },
                           new GeoFips
                           {
                               Description = "4 Hartford, CT ",
                               Val = "09003",
                           },
                           new GeoFips
                           {
                               Description = "4 Litchfield, CT ",
                               Val = "09005",
                           },
                           new GeoFips
                           {
                               Description = "4 Middlesex, CT ",
                               Val = "09007",
                           },
                           new GeoFips
                           {
                               Description = "4 New Haven, CT ",
                               Val = "09009",
                           },
                           new GeoFips
                           {
                               Description = "4 New London, CT ",
                               Val = "09011",
                           },
                           new GeoFips
                           {
                               Description = "4 Tolland, CT ",
                               Val = "09013",
                           },
                           new GeoFips
                           {
                               Description = "4 Windham, CT ",
                               Val = "09015",
                           },
                           new GeoFips
                           {
                               Description = "3 Delaware",
                               Val = "10000",
                           },
                           new GeoFips
                           {
                               Description = "4 Kent, DE ",
                               Val = "10001",
                           },
                           new GeoFips
                           {
                               Description = "4 New Castle, DE ",
                               Val = "10003",
                           },
                           new GeoFips
                           {
                               Description = "4 Sussex, DE ",
                               Val = "10005",
                           },
                           new GeoFips
                           {
                               Description = "5 Abilene, TX (Metropolitan Statistical Area)",
                               Val = "10180",
                           },
                           new GeoFips
                           {
                               Description = "5 Akron, OH (Metropolitan Statistical Area)",
                               Val = "10420",
                           },
                           new GeoFips
                           {
                               Description = "5 Albany, GA (Metropolitan Statistical Area)",
                               Val = "10500",
                           },
                           new GeoFips
                           {
                               Description = "5 Albany, OR (Metropolitan Statistical Area)",
                               Val = "10540",
                           },
                           new GeoFips
                           {
                               Description = "5 Albany-Schenectady-Troy, NY (Metropolitan Statistical Area)",
                               Val = "10580",
                           },
                           new GeoFips
                           {
                               Description = "5 Albuquerque, NM (Metropolitan Statistical Area)",
                               Val = "10740",
                           },
                           new GeoFips
                           {
                               Description = "5 Alexandria, LA (Metropolitan Statistical Area)",
                               Val = "10780",
                           },
                           new GeoFips
                           {
                               Description = "5 Allentown-Bethlehem-Easton, PA-NJ (Metropolitan Statistical Area)",
                               Val = "10900",
                           },
                           new GeoFips
                           {
                               Description = "3 District of Columbia",
                               Val = "11000",
                           },
                           new GeoFips
                           {
                               Description = "4 District of Columbia, DC ",
                               Val = "11001",
                           },
                           new GeoFips
                           {
                               Description = "5 Altoona, PA (Metropolitan Statistical Area)",
                               Val = "11020",
                           },
                           new GeoFips
                           {
                               Description = "5 Amarillo, TX (Metropolitan Statistical Area)",
                               Val = "11100",
                           },
                           new GeoFips
                           {
                               Description = "5 Ames, IA (Metropolitan Statistical Area)",
                               Val = "11180",
                           },
                           new GeoFips
                           {
                               Description = "5 Anchorage, AK (Metropolitan Statistical Area)",
                               Val = "11260",
                           },
                           new GeoFips
                           {
                               Description = "5 Ann Arbor, MI (Metropolitan Statistical Area)",
                               Val = "11460",
                           },
                           new GeoFips
                           {
                               Description = "5 Anniston-Oxford-Jacksonville, AL (Metropolitan Statistical Area)",
                               Val = "11500",
                           },
                           new GeoFips
                           {
                               Description = "5 Appleton, WI (Metropolitan Statistical Area)",
                               Val = "11540",
                           },
                           new GeoFips
                           {
                               Description = "5 Asheville, NC (Metropolitan Statistical Area)",
                               Val = "11700",
                           },
                           new GeoFips
                           {
                               Description = "3 Florida",
                               Val = "12000",
                           },
                           new GeoFips
                           {
                               Description = "4 Alachua, FL ",
                               Val = "12001",
                           },
                           new GeoFips
                           {
                               Description = "4 Baker, FL ",
                               Val = "12003",
                           },
                           new GeoFips
                           {
                               Description = "4 Bay, FL ",
                               Val = "12005",
                           },
                           new GeoFips
                           {
                               Description = "4 Bradford, FL ",
                               Val = "12007",
                           },
                           new GeoFips
                           {
                               Description = "4 Brevard, FL ",
                               Val = "12009",
                           },
                           new GeoFips
                           {
                               Description = "4 Broward, FL ",
                               Val = "12011",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, FL ",
                               Val = "12013",
                           },
                           new GeoFips
                           {
                               Description = "4 Charlotte, FL ",
                               Val = "12015",
                           },
                           new GeoFips
                           {
                               Description = "4 Citrus, FL ",
                               Val = "12017",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, FL ",
                               Val = "12019",
                           },
                           new GeoFips
                           {
                               Description = "5 Athens-Clarke County, GA (Metropolitan Statistical Area)",
                               Val = "12020",
                           },
                           new GeoFips
                           {
                               Description = "4 Collier, FL ",
                               Val = "12021",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbia, FL ",
                               Val = "12023",
                           },
                           new GeoFips
                           {
                               Description = "4 DeSoto, FL ",
                               Val = "12027",
                           },
                           new GeoFips
                           {
                               Description = "4 Dixie, FL ",
                               Val = "12029",
                           },
                           new GeoFips
                           {
                               Description = "4 Duval, FL ",
                               Val = "12031",
                           },
                           new GeoFips
                           {
                               Description = "4 Escambia, FL ",
                               Val = "12033",
                           },
                           new GeoFips
                           {
                               Description = "4 Flagler, FL ",
                               Val = "12035",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, FL ",
                               Val = "12037",
                           },
                           new GeoFips
                           {
                               Description = "4 Gadsden, FL ",
                               Val = "12039",
                           },
                           new GeoFips
                           {
                               Description = "4 Gilchrist, FL ",
                               Val = "12041",
                           },
                           new GeoFips
                           {
                               Description = "4 Glades, FL ",
                               Val = "12043",
                           },
                           new GeoFips
                           {
                               Description = "4 Gulf, FL ",
                               Val = "12045",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, FL ",
                               Val = "12047",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardee, FL ",
                               Val = "12049",
                           },
                           new GeoFips
                           {
                               Description = "4 Hendry, FL ",
                               Val = "12051",
                           },
                           new GeoFips
                           {
                               Description = "4 Hernando, FL ",
                               Val = "12053",
                           },
                           new GeoFips
                           {
                               Description = "4 Highlands, FL ",
                               Val = "12055",
                           },
                           new GeoFips
                           {
                               Description = "4 Hillsborough, FL ",
                               Val = "12057",
                           },
                           new GeoFips
                           {
                               Description = "4 Holmes, FL ",
                               Val = "12059",
                           },
                           new GeoFips
                           {
                               Description = "5 Atlanta-Sandy Springs-Roswell, GA (Metropolitan Statistical Area)",
                               Val = "12060",
                           },
                           new GeoFips
                           {
                               Description = "4 Indian River, FL ",
                               Val = "12061",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, FL ",
                               Val = "12063",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, FL ",
                               Val = "12065",
                           },
                           new GeoFips
                           {
                               Description = "4 Lafayette, FL ",
                               Val = "12067",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, FL ",
                               Val = "12069",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, FL ",
                               Val = "12071",
                           },
                           new GeoFips
                           {
                               Description = "4 Leon, FL ",
                               Val = "12073",
                           },
                           new GeoFips
                           {
                               Description = "4 Levy, FL ",
                               Val = "12075",
                           },
                           new GeoFips
                           {
                               Description = "4 Liberty, FL ",
                               Val = "12077",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, FL ",
                               Val = "12079",
                           },
                           new GeoFips
                           {
                               Description = "4 Manatee, FL ",
                               Val = "12081",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, FL ",
                               Val = "12083",
                           },
                           new GeoFips
                           {
                               Description = "4 Martin, FL ",
                               Val = "12085",
                           },
                           new GeoFips
                           {
                               Description = "4 Miami-Dade, FL ",
                               Val = "12086",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, FL ",
                               Val = "12087",
                           },
                           new GeoFips
                           {
                               Description = "4 Nassau, FL ",
                               Val = "12089",
                           },
                           new GeoFips
                           {
                               Description = "4 Okaloosa, FL ",
                               Val = "12091",
                           },
                           new GeoFips
                           {
                               Description = "4 Okeechobee, FL ",
                               Val = "12093",
                           },
                           new GeoFips
                           {
                               Description = "4 Orange, FL ",
                               Val = "12095",
                           },
                           new GeoFips
                           {
                               Description = "4 Osceola, FL ",
                               Val = "12097",
                           },
                           new GeoFips
                           {
                               Description = "4 Palm Beach, FL ",
                               Val = "12099",
                           },
                           new GeoFips
                           {
                               Description = "5 Atlantic City-Hammonton, NJ (Metropolitan Statistical Area)",
                               Val = "12100",
                           },
                           new GeoFips
                           {
                               Description = "4 Pasco, FL ",
                               Val = "12101",
                           },
                           new GeoFips
                           {
                               Description = "4 Pinellas, FL ",
                               Val = "12103",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, FL ",
                               Val = "12105",
                           },
                           new GeoFips
                           {
                               Description = "4 Putnam, FL ",
                               Val = "12107",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Johns, FL ",
                               Val = "12109",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Lucie, FL ",
                               Val = "12111",
                           },
                           new GeoFips
                           {
                               Description = "4 Santa Rosa, FL ",
                               Val = "12113",
                           },
                           new GeoFips
                           {
                               Description = "4 Sarasota, FL ",
                               Val = "12115",
                           },
                           new GeoFips
                           {
                               Description = "4 Seminole, FL ",
                               Val = "12117",
                           },
                           new GeoFips
                           {
                               Description = "4 Sumter, FL ",
                               Val = "12119",
                           },
                           new GeoFips
                           {
                               Description = "4 Suwannee, FL ",
                               Val = "12121",
                           },
                           new GeoFips
                           {
                               Description = "4 Taylor, FL ",
                               Val = "12123",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, FL ",
                               Val = "12125",
                           },
                           new GeoFips
                           {
                               Description = "4 Volusia, FL ",
                               Val = "12127",
                           },
                           new GeoFips
                           {
                               Description = "4 Wakulla, FL ",
                               Val = "12129",
                           },
                           new GeoFips
                           {
                               Description = "4 Walton, FL ",
                               Val = "12131",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, FL ",
                               Val = "12133",
                           },
                           new GeoFips
                           {
                               Description = "5 Auburn-Opelika, AL (Metropolitan Statistical Area)",
                               Val = "12220",
                           },
                           new GeoFips
                           {
                               Description = "5 Augusta-Richmond County, GA-SC (Metropolitan Statistical Area)",
                               Val = "12260",
                           },
                           new GeoFips
                           {
                               Description = "5 Austin-Round Rock, TX (Metropolitan Statistical Area)",
                               Val = "12420",
                           },
                           new GeoFips
                           {
                               Description = "5 Bakersfield, CA (Metropolitan Statistical Area)",
                               Val = "12540",
                           },
                           new GeoFips
                           {
                               Description = "5 Baltimore-Columbia-Towson, MD (Metropolitan Statistical Area)",
                               Val = "12580",
                           },
                           new GeoFips
                           {
                               Description = "5 Bangor, ME (Metropolitan Statistical Area)",
                               Val = "12620",
                           },
                           new GeoFips
                           {
                               Description = "5 Barnstable Town, MA (Metropolitan Statistical Area)",
                               Val = "12700",
                           },
                           new GeoFips
                           {
                               Description = "5 Baton Rouge, LA (Metropolitan Statistical Area)",
                               Val = "12940",
                           },
                           new GeoFips
                           {
                               Description = "5 Battle Creek, MI (Metropolitan Statistical Area)",
                               Val = "12980",
                           },
                           new GeoFips
                           {
                               Description = "3 Georgia",
                               Val = "13000",
                           },
                           new GeoFips
                           {
                               Description = "4 Appling, GA ",
                               Val = "13001",
                           },
                           new GeoFips
                           {
                               Description = "4 Atkinson, GA ",
                               Val = "13003",
                           },
                           new GeoFips
                           {
                               Description = "4 Bacon, GA ",
                               Val = "13005",
                           },
                           new GeoFips
                           {
                               Description = "4 Baker, GA ",
                               Val = "13007",
                           },
                           new GeoFips
                           {
                               Description = "4 Baldwin, GA ",
                               Val = "13009",
                           },
                           new GeoFips
                           {
                               Description = "4 Banks, GA ",
                               Val = "13011",
                           },
                           new GeoFips
                           {
                               Description = "4 Barrow, GA ",
                               Val = "13013",
                           },
                           new GeoFips
                           {
                               Description = "4 Bartow, GA ",
                               Val = "13015",
                           },
                           new GeoFips
                           {
                               Description = "4 Ben Hill, GA ",
                               Val = "13017",
                           },
                           new GeoFips
                           {
                               Description = "4 Berrien, GA ",
                               Val = "13019",
                           },
                           new GeoFips
                           {
                               Description = "5 Bay City, MI (Metropolitan Statistical Area)",
                               Val = "13020",
                           },
                           new GeoFips
                           {
                               Description = "4 Bibb, GA ",
                               Val = "13021",
                           },
                           new GeoFips
                           {
                               Description = "4 Bleckley, GA ",
                               Val = "13023",
                           },
                           new GeoFips
                           {
                               Description = "4 Brantley, GA ",
                               Val = "13025",
                           },
                           new GeoFips
                           {
                               Description = "4 Brooks, GA ",
                               Val = "13027",
                           },
                           new GeoFips
                           {
                               Description = "4 Bryan, GA ",
                               Val = "13029",
                           },
                           new GeoFips
                           {
                               Description = "4 Bulloch, GA ",
                               Val = "13031",
                           },
                           new GeoFips
                           {
                               Description = "4 Burke, GA ",
                               Val = "13033",
                           },
                           new GeoFips
                           {
                               Description = "4 Butts, GA ",
                               Val = "13035",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, GA ",
                               Val = "13037",
                           },
                           new GeoFips
                           {
                               Description = "4 Camden, GA ",
                               Val = "13039",
                           },
                           new GeoFips
                           {
                               Description = "4 Candler, GA ",
                               Val = "13043",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, GA ",
                               Val = "13045",
                           },
                           new GeoFips
                           {
                               Description = "4 Catoosa, GA ",
                               Val = "13047",
                           },
                           new GeoFips
                           {
                               Description = "4 Charlton, GA ",
                               Val = "13049",
                           },
                           new GeoFips
                           {
                               Description = "4 Chatham, GA ",
                               Val = "13051",
                           },
                           new GeoFips
                           {
                               Description = "4 Chattahoochee, GA ",
                               Val = "13053",
                           },
                           new GeoFips
                           {
                               Description = "4 Chattooga, GA ",
                               Val = "13055",
                           },
                           new GeoFips
                           {
                               Description = "4 Cherokee, GA ",
                               Val = "13057",
                           },
                           new GeoFips
                           {
                               Description = "4 Clarke, GA ",
                               Val = "13059",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, GA ",
                               Val = "13061",
                           },
                           new GeoFips
                           {
                               Description = "4 Clayton, GA ",
                               Val = "13063",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinch, GA ",
                               Val = "13065",
                           },
                           new GeoFips
                           {
                               Description = "4 Cobb, GA ",
                               Val = "13067",
                           },
                           new GeoFips
                           {
                               Description = "4 Coffee, GA ",
                               Val = "13069",
                           },
                           new GeoFips
                           {
                               Description = "4 Colquitt, GA ",
                               Val = "13071",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbia, GA ",
                               Val = "13073",
                           },
                           new GeoFips
                           {
                               Description = "4 Cook, GA ",
                               Val = "13075",
                           },
                           new GeoFips
                           {
                               Description = "4 Coweta, GA ",
                               Val = "13077",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, GA ",
                               Val = "13079",
                           },
                           new GeoFips
                           {
                               Description = "4 Crisp, GA ",
                               Val = "13081",
                           },
                           new GeoFips
                           {
                               Description = "4 Dade, GA ",
                               Val = "13083",
                           },
                           new GeoFips
                           {
                               Description = "4 Dawson, GA ",
                               Val = "13085",
                           },
                           new GeoFips
                           {
                               Description = "4 Decatur, GA ",
                               Val = "13087",
                           },
                           new GeoFips
                           {
                               Description = "4 DeKalb, GA ",
                               Val = "13089",
                           },
                           new GeoFips
                           {
                               Description = "4 Dodge, GA ",
                               Val = "13091",
                           },
                           new GeoFips
                           {
                               Description = "4 Dooly, GA ",
                               Val = "13093",
                           },
                           new GeoFips
                           {
                               Description = "4 Dougherty, GA ",
                               Val = "13095",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, GA ",
                               Val = "13097",
                           },
                           new GeoFips
                           {
                               Description = "4 Early, GA ",
                               Val = "13099",
                           },
                           new GeoFips
                           {
                               Description = "4 Echols, GA ",
                               Val = "13101",
                           },
                           new GeoFips
                           {
                               Description = "4 Effingham, GA ",
                               Val = "13103",
                           },
                           new GeoFips
                           {
                               Description = "4 Elbert, GA ",
                               Val = "13105",
                           },
                           new GeoFips
                           {
                               Description = "4 Emanuel, GA ",
                               Val = "13107",
                           },
                           new GeoFips
                           {
                               Description = "4 Evans, GA ",
                               Val = "13109",
                           },
                           new GeoFips
                           {
                               Description = "4 Fannin, GA ",
                               Val = "13111",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, GA ",
                               Val = "13113",
                           },
                           new GeoFips
                           {
                               Description = "4 Floyd, GA ",
                               Val = "13115",
                           },
                           new GeoFips
                           {
                               Description = "4 Forsyth, GA ",
                               Val = "13117",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, GA ",
                               Val = "13119",
                           },
                           new GeoFips
                           {
                               Description = "4 Fulton, GA ",
                               Val = "13121",
                           },
                           new GeoFips
                           {
                               Description = "4 Gilmer, GA ",
                               Val = "13123",
                           },
                           new GeoFips
                           {
                               Description = "4 Glascock, GA ",
                               Val = "13125",
                           },
                           new GeoFips
                           {
                               Description = "4 Glynn, GA ",
                               Val = "13127",
                           },
                           new GeoFips
                           {
                               Description = "4 Gordon, GA ",
                               Val = "13129",
                           },
                           new GeoFips
                           {
                               Description = "4 Grady, GA ",
                               Val = "13131",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, GA ",
                               Val = "13133",
                           },
                           new GeoFips
                           {
                               Description = "4 Gwinnett, GA ",
                               Val = "13135",
                           },
                           new GeoFips
                           {
                               Description = "4 Habersham, GA ",
                               Val = "13137",
                           },
                           new GeoFips
                           {
                               Description = "4 Hall, GA ",
                               Val = "13139",
                           },
                           new GeoFips
                           {
                               Description = "5 Beaumont-Port Arthur, TX (Metropolitan Statistical Area)",
                               Val = "13140",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, GA ",
                               Val = "13141",
                           },
                           new GeoFips
                           {
                               Description = "4 Haralson, GA ",
                               Val = "13143",
                           },
                           new GeoFips
                           {
                               Description = "4 Harris, GA ",
                               Val = "13145",
                           },
                           new GeoFips
                           {
                               Description = "4 Hart, GA ",
                               Val = "13147",
                           },
                           new GeoFips
                           {
                               Description = "4 Heard, GA ",
                               Val = "13149",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry, GA ",
                               Val = "13151",
                           },
                           new GeoFips
                           {
                               Description = "4 Houston, GA ",
                               Val = "13153",
                           },
                           new GeoFips
                           {
                               Description = "4 Irwin, GA ",
                               Val = "13155",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, GA ",
                               Val = "13157",
                           },
                           new GeoFips
                           {
                               Description = "4 Jasper, GA ",
                               Val = "13159",
                           },
                           new GeoFips
                           {
                               Description = "4 Jeff Davis, GA ",
                               Val = "13161",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, GA ",
                               Val = "13163",
                           },
                           new GeoFips
                           {
                               Description = "4 Jenkins, GA ",
                               Val = "13165",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, GA ",
                               Val = "13167",
                           },
                           new GeoFips
                           {
                               Description = "4 Jones, GA ",
                               Val = "13169",
                           },
                           new GeoFips
                           {
                               Description = "4 Lamar, GA ",
                               Val = "13171",
                           },
                           new GeoFips
                           {
                               Description = "4 Lanier, GA ",
                               Val = "13173",
                           },
                           new GeoFips
                           {
                               Description = "4 Laurens, GA ",
                               Val = "13175",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, GA ",
                               Val = "13177",
                           },
                           new GeoFips
                           {
                               Description = "4 Liberty, GA ",
                               Val = "13179",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, GA ",
                               Val = "13181",
                           },
                           new GeoFips
                           {
                               Description = "4 Long, GA ",
                               Val = "13183",
                           },
                           new GeoFips
                           {
                               Description = "4 Lowndes, GA ",
                               Val = "13185",
                           },
                           new GeoFips
                           {
                               Description = "4 Lumpkin, GA ",
                               Val = "13187",
                           },
                           new GeoFips
                           {
                               Description = "4 McDuffie, GA ",
                               Val = "13189",
                           },
                           new GeoFips
                           {
                               Description = "4 McIntosh, GA ",
                               Val = "13191",
                           },
                           new GeoFips
                           {
                               Description = "4 Macon, GA ",
                               Val = "13193",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, GA ",
                               Val = "13195",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, GA ",
                               Val = "13197",
                           },
                           new GeoFips
                           {
                               Description = "4 Meriwether, GA ",
                               Val = "13199",
                           },
                           new GeoFips
                           {
                               Description = "4 Miller, GA ",
                               Val = "13201",
                           },
                           new GeoFips
                           {
                               Description = "4 Mitchell, GA ",
                               Val = "13205",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, GA ",
                               Val = "13207",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, GA ",
                               Val = "13209",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, GA ",
                               Val = "13211",
                           },
                           new GeoFips
                           {
                               Description = "4 Murray, GA ",
                               Val = "13213",
                           },
                           new GeoFips
                           {
                               Description = "4 Muscogee, GA ",
                               Val = "13215",
                           },
                           new GeoFips
                           {
                               Description = "4 Newton, GA ",
                               Val = "13217",
                           },
                           new GeoFips
                           {
                               Description = "4 Oconee, GA ",
                               Val = "13219",
                           },
                           new GeoFips
                           {
                               Description = "5 Beckley, WV (Metropolitan Statistical Area)",
                               Val = "13220",
                           },
                           new GeoFips
                           {
                               Description = "4 Oglethorpe, GA ",
                               Val = "13221",
                           },
                           new GeoFips
                           {
                               Description = "4 Paulding, GA ",
                               Val = "13223",
                           },
                           new GeoFips
                           {
                               Description = "4 Peach, GA ",
                               Val = "13225",
                           },
                           new GeoFips
                           {
                               Description = "4 Pickens, GA ",
                               Val = "13227",
                           },
                           new GeoFips
                           {
                               Description = "4 Pierce, GA ",
                               Val = "13229",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, GA ",
                               Val = "13231",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, GA ",
                               Val = "13233",
                           },
                           new GeoFips
                           {
                               Description = "4 Pulaski, GA ",
                               Val = "13235",
                           },
                           new GeoFips
                           {
                               Description = "4 Putnam, GA ",
                               Val = "13237",
                           },
                           new GeoFips
                           {
                               Description = "4 Quitman, GA ",
                               Val = "13239",
                           },
                           new GeoFips
                           {
                               Description = "4 Rabun, GA ",
                               Val = "13241",
                           },
                           new GeoFips
                           {
                               Description = "4 Randolph, GA ",
                               Val = "13243",
                           },
                           new GeoFips
                           {
                               Description = "4 Richmond, GA ",
                               Val = "13245",
                           },
                           new GeoFips
                           {
                               Description = "4 Rockdale, GA ",
                               Val = "13247",
                           },
                           new GeoFips
                           {
                               Description = "4 Schley, GA ",
                               Val = "13249",
                           },
                           new GeoFips
                           {
                               Description = "4 Screven, GA ",
                               Val = "13251",
                           },
                           new GeoFips
                           {
                               Description = "4 Seminole, GA ",
                               Val = "13253",
                           },
                           new GeoFips
                           {
                               Description = "4 Spalding, GA ",
                               Val = "13255",
                           },
                           new GeoFips
                           {
                               Description = "4 Stephens, GA ",
                               Val = "13257",
                           },
                           new GeoFips
                           {
                               Description = "4 Stewart, GA ",
                               Val = "13259",
                           },
                           new GeoFips
                           {
                               Description = "4 Sumter, GA ",
                               Val = "13261",
                           },
                           new GeoFips
                           {
                               Description = "4 Talbot, GA ",
                               Val = "13263",
                           },
                           new GeoFips
                           {
                               Description = "4 Taliaferro, GA ",
                               Val = "13265",
                           },
                           new GeoFips
                           {
                               Description = "4 Tattnall, GA ",
                               Val = "13267",
                           },
                           new GeoFips
                           {
                               Description = "4 Taylor, GA ",
                               Val = "13269",
                           },
                           new GeoFips
                           {
                               Description = "4 Telfair, GA ",
                               Val = "13271",
                           },
                           new GeoFips
                           {
                               Description = "4 Terrell, GA ",
                               Val = "13273",
                           },
                           new GeoFips
                           {
                               Description = "4 Thomas, GA ",
                               Val = "13275",
                           },
                           new GeoFips
                           {
                               Description = "4 Tift, GA ",
                               Val = "13277",
                           },
                           new GeoFips
                           {
                               Description = "4 Toombs, GA ",
                               Val = "13279",
                           },
                           new GeoFips
                           {
                               Description = "4 Towns, GA ",
                               Val = "13281",
                           },
                           new GeoFips
                           {
                               Description = "4 Treutlen, GA ",
                               Val = "13283",
                           },
                           new GeoFips
                           {
                               Description = "4 Troup, GA ",
                               Val = "13285",
                           },
                           new GeoFips
                           {
                               Description = "4 Turner, GA ",
                               Val = "13287",
                           },
                           new GeoFips
                           {
                               Description = "4 Twiggs, GA ",
                               Val = "13289",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, GA ",
                               Val = "13291",
                           },
                           new GeoFips
                           {
                               Description = "4 Upson, GA ",
                               Val = "13293",
                           },
                           new GeoFips
                           {
                               Description = "4 Walker, GA ",
                               Val = "13295",
                           },
                           new GeoFips
                           {
                               Description = "4 Walton, GA ",
                               Val = "13297",
                           },
                           new GeoFips
                           {
                               Description = "4 Ware, GA ",
                               Val = "13299",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, GA ",
                               Val = "13301",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, GA ",
                               Val = "13303",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, GA ",
                               Val = "13305",
                           },
                           new GeoFips
                           {
                               Description = "4 Webster, GA ",
                               Val = "13307",
                           },
                           new GeoFips
                           {
                               Description = "4 Wheeler, GA ",
                               Val = "13309",
                           },
                           new GeoFips
                           {
                               Description = "4 White, GA ",
                               Val = "13311",
                           },
                           new GeoFips
                           {
                               Description = "4 Whitfield, GA ",
                               Val = "13313",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilcox, GA ",
                               Val = "13315",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilkes, GA ",
                               Val = "13317",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilkinson, GA ",
                               Val = "13319",
                           },
                           new GeoFips
                           {
                               Description = "4 Worth, GA ",
                               Val = "13321",
                           },
                           new GeoFips
                           {
                               Description = "5 Bellingham, WA (Metropolitan Statistical Area)",
                               Val = "13380",
                           },
                           new GeoFips
                           {
                               Description = "5 Bend-Redmond, OR (Metropolitan Statistical Area)",
                               Val = "13460",
                           },
                           new GeoFips
                           {
                               Description = "5 Billings, MT (Metropolitan Statistical Area)",
                               Val = "13740",
                           },
                           new GeoFips
                           {
                               Description = "5 Binghamton, NY (Metropolitan Statistical Area)",
                               Val = "13780",
                           },
                           new GeoFips
                           {
                               Description = "5 Birmingham-Hoover, AL (Metropolitan Statistical Area)",
                               Val = "13820",
                           },
                           new GeoFips
                           {
                               Description = "5 Bismarck, ND (Metropolitan Statistical Area)",
                               Val = "13900",
                           },
                           new GeoFips
                           {
                               Description = "5 Blacksburg-Christiansburg-Radford, VA (Metropolitan Statistical Area)",
                               Val = "13980",
                           },
                           new GeoFips
                           {
                               Description = "5 Bloomington, IL (Metropolitan Statistical Area)",
                               Val = "14010",
                           },
                           new GeoFips
                           {
                               Description = "5 Bloomington, IN (Metropolitan Statistical Area)",
                               Val = "14020",
                           },
                           new GeoFips
                           {
                               Description = "5 Bloomsburg-Berwick, PA (Metropolitan Statistical Area)",
                               Val = "14100",
                           },
                           new GeoFips
                           {
                               Description = "5 Boise City, ID (Metropolitan Statistical Area)",
                               Val = "14260",
                           },
                           new GeoFips
                           {
                               Description = "5 Boston-Cambridge-Newton, MA-NH (Metropolitan Statistical Area)",
                               Val = "14460",
                           },
                           new GeoFips
                           {
                               Description = "5 Boulder, CO (Metropolitan Statistical Area)",
                               Val = "14500",
                           },
                           new GeoFips
                           {
                               Description = "5 Bowling Green, KY (Metropolitan Statistical Area)",
                               Val = "14540",
                           },
                           new GeoFips
                           {
                               Description = "5 Bremerton-Silverdale, WA (Metropolitan Statistical Area)",
                               Val = "14740",
                           },
                           new GeoFips
                           {
                               Description = "5 Bridgeport-Stamford-Norwalk, CT (Metropolitan Statistical Area)",
                               Val = "14860",
                           },
                           new GeoFips
                           {
                               Description = "3 Hawaii",
                               Val = "15000",
                           },
                           new GeoFips
                           {
                               Description = "4 Hawaii, HI ",
                               Val = "15001",
                           },
                           new GeoFips
                           {
                               Description = "4 Honolulu, HI ",
                               Val = "15003",
                           },
                           new GeoFips
                           {
                               Description = "4 Kauai, HI ",
                               Val = "15007",
                           },
                           new GeoFips
                           {
                               Description = "5 Brownsville-Harlingen, TX (Metropolitan Statistical Area)",
                               Val = "15180",
                           },
                           new GeoFips
                           {
                               Description = "5 Brunswick, GA (Metropolitan Statistical Area)",
                               Val = "15260",
                           },
                           new GeoFips
                           {
                               Description = "5 Buffalo-Cheektowaga-Niagara Falls, NY (Metropolitan Statistical Area)",
                               Val = "15380",
                           },
                           new GeoFips
                           {
                               Description = "5 Burlington, NC (Metropolitan Statistical Area)",
                               Val = "15500",
                           },
                           new GeoFips
                           {
                               Description = "5 Burlington-South Burlington, VT (Metropolitan Statistical Area)",
                               Val = "15540",
                           },
                           new GeoFips
                           {
                               Description = "5 California-Lexington Park, MD (Metropolitan Statistical Area)",
                               Val = "15680",
                           },
                           new GeoFips
                           {
                               Description = "4 Maui + Kalawao, HI ",
                               Val = "15901",
                           },
                           new GeoFips
                           {
                               Description = "5 Canton-Massillon, OH (Metropolitan Statistical Area)",
                               Val = "15940",
                           },
                           new GeoFips
                           {
                               Description = "5 Cape Coral-Fort Myers, FL (Metropolitan Statistical Area)",
                               Val = "15980",
                           },
                           new GeoFips
                           {
                               Description = "3 Idaho",
                               Val = "16000",
                           },
                           new GeoFips
                           {
                               Description = "4 Ada, ID ",
                               Val = "16001",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, ID ",
                               Val = "16003",
                           },
                           new GeoFips
                           {
                               Description = "4 Bannock, ID ",
                               Val = "16005",
                           },
                           new GeoFips
                           {
                               Description = "4 Bear Lake, ID ",
                               Val = "16007",
                           },
                           new GeoFips
                           {
                               Description = "4 Benewah, ID ",
                               Val = "16009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bingham, ID ",
                               Val = "16011",
                           },
                           new GeoFips
                           {
                               Description = "4 Blaine, ID ",
                               Val = "16013",
                           },
                           new GeoFips
                           {
                               Description = "4 Boise, ID ",
                               Val = "16015",
                           },
                           new GeoFips
                           {
                               Description = "4 Bonner, ID ",
                               Val = "16017",
                           },
                           new GeoFips
                           {
                               Description = "4 Bonneville, ID ",
                               Val = "16019",
                           },
                           new GeoFips
                           {
                               Description = "5 Cape Girardeau, MO-IL (Metropolitan Statistical Area)",
                               Val = "16020",
                           },
                           new GeoFips
                           {
                               Description = "4 Boundary, ID ",
                               Val = "16021",
                           },
                           new GeoFips
                           {
                               Description = "4 Butte, ID ",
                               Val = "16023",
                           },
                           new GeoFips
                           {
                               Description = "4 Camas, ID ",
                               Val = "16025",
                           },
                           new GeoFips
                           {
                               Description = "4 Canyon, ID ",
                               Val = "16027",
                           },
                           new GeoFips
                           {
                               Description = "4 Caribou, ID ",
                               Val = "16029",
                           },
                           new GeoFips
                           {
                               Description = "4 Cassia, ID ",
                               Val = "16031",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, ID ",
                               Val = "16033",
                           },
                           new GeoFips
                           {
                               Description = "4 Clearwater, ID ",
                               Val = "16035",
                           },
                           new GeoFips
                           {
                               Description = "4 Custer, ID ",
                               Val = "16037",
                           },
                           new GeoFips
                           {
                               Description = "4 Elmore, ID ",
                               Val = "16039",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, ID ",
                               Val = "16041",
                           },
                           new GeoFips
                           {
                               Description = "4 Fremont (includes Yellowstone Park), ID ",
                               Val = "16043",
                           },
                           new GeoFips
                           {
                               Description = "4 Gem, ID ",
                               Val = "16045",
                           },
                           new GeoFips
                           {
                               Description = "4 Gooding, ID ",
                               Val = "16047",
                           },
                           new GeoFips
                           {
                               Description = "4 Idaho, ID ",
                               Val = "16049",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, ID ",
                               Val = "16051",
                           },
                           new GeoFips
                           {
                               Description = "4 Jerome, ID ",
                               Val = "16053",
                           },
                           new GeoFips
                           {
                               Description = "4 Kootenai, ID ",
                               Val = "16055",
                           },
                           new GeoFips
                           {
                               Description = "4 Latah, ID ",
                               Val = "16057",
                           },
                           new GeoFips
                           {
                               Description = "4 Lemhi, ID ",
                               Val = "16059",
                           },
                           new GeoFips
                           {
                               Description = "5 Carbondale-Marion, IL (Metropolitan Statistical Area)",
                               Val = "16060",
                           },
                           new GeoFips
                           {
                               Description = "4 Lewis, ID ",
                               Val = "16061",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, ID ",
                               Val = "16063",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, ID ",
                               Val = "16065",
                           },
                           new GeoFips
                           {
                               Description = "4 Minidoka, ID ",
                               Val = "16067",
                           },
                           new GeoFips
                           {
                               Description = "4 Nez Perce, ID ",
                               Val = "16069",
                           },
                           new GeoFips
                           {
                               Description = "4 Oneida, ID ",
                               Val = "16071",
                           },
                           new GeoFips
                           {
                               Description = "4 Owyhee, ID ",
                               Val = "16073",
                           },
                           new GeoFips
                           {
                               Description = "4 Payette, ID ",
                               Val = "16075",
                           },
                           new GeoFips
                           {
                               Description = "4 Power, ID ",
                               Val = "16077",
                           },
                           new GeoFips
                           {
                               Description = "4 Shoshone, ID ",
                               Val = "16079",
                           },
                           new GeoFips
                           {
                               Description = "4 Teton, ID ",
                               Val = "16081",
                           },
                           new GeoFips
                           {
                               Description = "4 Twin Falls, ID ",
                               Val = "16083",
                           },
                           new GeoFips
                           {
                               Description = "4 Valley, ID ",
                               Val = "16085",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, ID ",
                               Val = "16087",
                           },
                           new GeoFips
                           {
                               Description = "5 Carson City, NV (Metropolitan Statistical Area)",
                               Val = "16180",
                           },
                           new GeoFips
                           {
                               Description = "5 Casper, WY (Metropolitan Statistical Area)",
                               Val = "16220",
                           },
                           new GeoFips
                           {
                               Description = "5 Cedar Rapids, IA (Metropolitan Statistical Area)",
                               Val = "16300",
                           },
                           new GeoFips
                           {
                               Description = "5 Chambersburg-Waynesboro, PA (Metropolitan Statistical Area)",
                               Val = "16540",
                           },
                           new GeoFips
                           {
                               Description = "5 Champaign-Urbana, IL (Metropolitan Statistical Area)",
                               Val = "16580",
                           },
                           new GeoFips
                           {
                               Description = "5 Charleston, WV (Metropolitan Statistical Area)",
                               Val = "16620",
                           },
                           new GeoFips
                           {
                               Description = "5 Charleston-North Charleston, SC (Metropolitan Statistical Area)",
                               Val = "16700",
                           },
                           new GeoFips
                           {
                               Description = "5 Charlotte-Concord-Gastonia, NC-SC (Metropolitan Statistical Area)",
                               Val = "16740",
                           },
                           new GeoFips
                           {
                               Description = "5 Charlottesville, VA (Metropolitan Statistical Area)",
                               Val = "16820",
                           },
                           new GeoFips
                           {
                               Description = "5 Chattanooga, TN-GA (Metropolitan Statistical Area)",
                               Val = "16860",
                           },
                           new GeoFips
                           {
                               Description = "5 Cheyenne, WY (Metropolitan Statistical Area)",
                               Val = "16940",
                           },
                           new GeoFips
                           {
                               Description = "5 Chicago-Naperville-Elgin, IL-IN-WI (Metropolitan Statistical Area)",
                               Val = "16980",
                           },
                           new GeoFips
                           {
                               Description = "3 Illinois",
                               Val = "17000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, IL ",
                               Val = "17001",
                           },
                           new GeoFips
                           {
                               Description = "4 Alexander, IL ",
                               Val = "17003",
                           },
                           new GeoFips
                           {
                               Description = "4 Bond, IL ",
                               Val = "17005",
                           },
                           new GeoFips
                           {
                               Description = "4 Boone, IL ",
                               Val = "17007",
                           },
                           new GeoFips
                           {
                               Description = "4 Brown, IL ",
                               Val = "17009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bureau, IL ",
                               Val = "17011",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, IL ",
                               Val = "17013",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, IL ",
                               Val = "17015",
                           },
                           new GeoFips
                           {
                               Description = "4 Cass, IL ",
                               Val = "17017",
                           },
                           new GeoFips
                           {
                               Description = "4 Champaign, IL ",
                               Val = "17019",
                           },
                           new GeoFips
                           {
                               Description = "5 Chico, CA (Metropolitan Statistical Area)",
                               Val = "17020",
                           },
                           new GeoFips
                           {
                               Description = "4 Christian, IL ",
                               Val = "17021",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, IL ",
                               Val = "17023",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, IL ",
                               Val = "17025",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinton, IL ",
                               Val = "17027",
                           },
                           new GeoFips
                           {
                               Description = "4 Coles, IL ",
                               Val = "17029",
                           },
                           new GeoFips
                           {
                               Description = "4 Cook, IL ",
                               Val = "17031",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, IL ",
                               Val = "17033",
                           },
                           new GeoFips
                           {
                               Description = "4 Cumberland, IL ",
                               Val = "17035",
                           },
                           new GeoFips
                           {
                               Description = "4 DeKalb, IL ",
                               Val = "17037",
                           },
                           new GeoFips
                           {
                               Description = "4 De Witt, IL ",
                               Val = "17039",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, IL ",
                               Val = "17041",
                           },
                           new GeoFips
                           {
                               Description = "4 DuPage, IL ",
                               Val = "17043",
                           },
                           new GeoFips
                           {
                               Description = "4 Edgar, IL ",
                               Val = "17045",
                           },
                           new GeoFips
                           {
                               Description = "4 Edwards, IL ",
                               Val = "17047",
                           },
                           new GeoFips
                           {
                               Description = "4 Effingham, IL ",
                               Val = "17049",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, IL ",
                               Val = "17051",
                           },
                           new GeoFips
                           {
                               Description = "4 Ford, IL ",
                               Val = "17053",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, IL ",
                               Val = "17055",
                           },
                           new GeoFips
                           {
                               Description = "4 Fulton, IL ",
                               Val = "17057",
                           },
                           new GeoFips
                           {
                               Description = "4 Gallatin, IL ",
                               Val = "17059",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, IL ",
                               Val = "17061",
                           },
                           new GeoFips
                           {
                               Description = "4 Grundy, IL ",
                               Val = "17063",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, IL ",
                               Val = "17065",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, IL ",
                               Val = "17067",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardin, IL ",
                               Val = "17069",
                           },
                           new GeoFips
                           {
                               Description = "4 Henderson, IL ",
                               Val = "17071",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry, IL ",
                               Val = "17073",
                           },
                           new GeoFips
                           {
                               Description = "4 Iroquois, IL ",
                               Val = "17075",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, IL ",
                               Val = "17077",
                           },
                           new GeoFips
                           {
                               Description = "4 Jasper, IL ",
                               Val = "17079",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, IL ",
                               Val = "17081",
                           },
                           new GeoFips
                           {
                               Description = "4 Jersey, IL ",
                               Val = "17083",
                           },
                           new GeoFips
                           {
                               Description = "4 Jo Daviess, IL ",
                               Val = "17085",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, IL ",
                               Val = "17087",
                           },
                           new GeoFips
                           {
                               Description = "4 Kane, IL ",
                               Val = "17089",
                           },
                           new GeoFips
                           {
                               Description = "4 Kankakee, IL ",
                               Val = "17091",
                           },
                           new GeoFips
                           {
                               Description = "4 Kendall, IL ",
                               Val = "17093",
                           },
                           new GeoFips
                           {
                               Description = "4 Knox, IL ",
                               Val = "17095",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, IL ",
                               Val = "17097",
                           },
                           new GeoFips
                           {
                               Description = "4 LaSalle, IL ",
                               Val = "17099",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, IL ",
                               Val = "17101",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, IL ",
                               Val = "17103",
                           },
                           new GeoFips
                           {
                               Description = "4 Livingston, IL ",
                               Val = "17105",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, IL ",
                               Val = "17107",
                           },
                           new GeoFips
                           {
                               Description = "4 McDonough, IL ",
                               Val = "17109",
                           },
                           new GeoFips
                           {
                               Description = "4 McHenry, IL ",
                               Val = "17111",
                           },
                           new GeoFips
                           {
                               Description = "4 McLean, IL ",
                               Val = "17113",
                           },
                           new GeoFips
                           {
                               Description = "4 Macon, IL ",
                               Val = "17115",
                           },
                           new GeoFips
                           {
                               Description = "4 Macoupin, IL ",
                               Val = "17117",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, IL ",
                               Val = "17119",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, IL ",
                               Val = "17121",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, IL ",
                               Val = "17123",
                           },
                           new GeoFips
                           {
                               Description = "4 Mason, IL ",
                               Val = "17125",
                           },
                           new GeoFips
                           {
                               Description = "4 Massac, IL ",
                               Val = "17127",
                           },
                           new GeoFips
                           {
                               Description = "4 Menard, IL ",
                               Val = "17129",
                           },
                           new GeoFips
                           {
                               Description = "4 Mercer, IL ",
                               Val = "17131",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, IL ",
                               Val = "17133",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, IL ",
                               Val = "17135",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, IL ",
                               Val = "17137",
                           },
                           new GeoFips
                           {
                               Description = "4 Moultrie, IL ",
                               Val = "17139",
                           },
                           new GeoFips
                           {
                               Description = "5 Cincinnati, OH-KY-IN (Metropolitan Statistical Area)",
                               Val = "17140",
                           },
                           new GeoFips
                           {
                               Description = "4 Ogle, IL ",
                               Val = "17141",
                           },
                           new GeoFips
                           {
                               Description = "4 Peoria, IL ",
                               Val = "17143",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, IL ",
                               Val = "17145",
                           },
                           new GeoFips
                           {
                               Description = "4 Piatt, IL ",
                               Val = "17147",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, IL ",
                               Val = "17149",
                           },
                           new GeoFips
                           {
                               Description = "4 Pope, IL ",
                               Val = "17151",
                           },
                           new GeoFips
                           {
                               Description = "4 Pulaski, IL ",
                               Val = "17153",
                           },
                           new GeoFips
                           {
                               Description = "4 Putnam, IL ",
                               Val = "17155",
                           },
                           new GeoFips
                           {
                               Description = "4 Randolph, IL ",
                               Val = "17157",
                           },
                           new GeoFips
                           {
                               Description = "4 Richland, IL ",
                               Val = "17159",
                           },
                           new GeoFips
                           {
                               Description = "4 Rock Island, IL ",
                               Val = "17161",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Clair, IL ",
                               Val = "17163",
                           },
                           new GeoFips
                           {
                               Description = "4 Saline, IL ",
                               Val = "17165",
                           },
                           new GeoFips
                           {
                               Description = "4 Sangamon, IL ",
                               Val = "17167",
                           },
                           new GeoFips
                           {
                               Description = "4 Schuyler, IL ",
                               Val = "17169",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, IL ",
                               Val = "17171",
                           },
                           new GeoFips
                           {
                               Description = "4 Shelby, IL ",
                               Val = "17173",
                           },
                           new GeoFips
                           {
                               Description = "4 Stark, IL ",
                               Val = "17175",
                           },
                           new GeoFips
                           {
                               Description = "4 Stephenson, IL ",
                               Val = "17177",
                           },
                           new GeoFips
                           {
                               Description = "4 Tazewell, IL ",
                               Val = "17179",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, IL ",
                               Val = "17181",
                           },
                           new GeoFips
                           {
                               Description = "4 Vermilion, IL ",
                               Val = "17183",
                           },
                           new GeoFips
                           {
                               Description = "4 Wabash, IL ",
                               Val = "17185",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, IL ",
                               Val = "17187",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, IL ",
                               Val = "17189",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, IL ",
                               Val = "17191",
                           },
                           new GeoFips
                           {
                               Description = "4 White, IL ",
                               Val = "17193",
                           },
                           new GeoFips
                           {
                               Description = "4 Whiteside, IL ",
                               Val = "17195",
                           },
                           new GeoFips
                           {
                               Description = "4 Will, IL ",
                               Val = "17197",
                           },
                           new GeoFips
                           {
                               Description = "4 Williamson, IL ",
                               Val = "17199",
                           },
                           new GeoFips
                           {
                               Description = "4 Winnebago, IL ",
                               Val = "17201",
                           },
                           new GeoFips
                           {
                               Description = "4 Woodford, IL ",
                               Val = "17203",
                           },
                           new GeoFips
                           {
                               Description = "5 Clarksville, TN-KY (Metropolitan Statistical Area)",
                               Val = "17300",
                           },
                           new GeoFips
                           {
                               Description = "5 Cleveland, TN (Metropolitan Statistical Area)",
                               Val = "17420",
                           },
                           new GeoFips
                           {
                               Description = "5 Cleveland-Elyria, OH (Metropolitan Statistical Area)",
                               Val = "17460",
                           },
                           new GeoFips
                           {
                               Description = "5 Coeur d'Alene, ID (Metropolitan Statistical Area)",
                               Val = "17660",
                           },
                           new GeoFips
                           {
                               Description = "5 College Station-Bryan, TX (Metropolitan Statistical Area)",
                               Val = "17780",
                           },
                           new GeoFips
                           {
                               Description = "5 Colorado Springs, CO (Metropolitan Statistical Area)",
                               Val = "17820",
                           },
                           new GeoFips
                           {
                               Description = "5 Columbia, MO (Metropolitan Statistical Area)",
                               Val = "17860",
                           },
                           new GeoFips
                           {
                               Description = "5 Columbia, SC (Metropolitan Statistical Area)",
                               Val = "17900",
                           },
                           new GeoFips
                           {
                               Description = "5 Columbus, GA-AL (Metropolitan Statistical Area)",
                               Val = "17980",
                           },
                           new GeoFips
                           {
                               Description = "3 Indiana",
                               Val = "18000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, IN ",
                               Val = "18001",
                           },
                           new GeoFips
                           {
                               Description = "4 Allen, IN ",
                               Val = "18003",
                           },
                           new GeoFips
                           {
                               Description = "4 Bartholomew, IN ",
                               Val = "18005",
                           },
                           new GeoFips
                           {
                               Description = "4 Benton, IN ",
                               Val = "18007",
                           },
                           new GeoFips
                           {
                               Description = "4 Blackford, IN ",
                               Val = "18009",
                           },
                           new GeoFips
                           {
                               Description = "4 Boone, IN ",
                               Val = "18011",
                           },
                           new GeoFips
                           {
                               Description = "4 Brown, IN ",
                               Val = "18013",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, IN ",
                               Val = "18015",
                           },
                           new GeoFips
                           {
                               Description = "4 Cass, IN ",
                               Val = "18017",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, IN ",
                               Val = "18019",
                           },
                           new GeoFips
                           {
                               Description = "5 Columbus, IN (Metropolitan Statistical Area)",
                               Val = "18020",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, IN ",
                               Val = "18021",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinton, IN ",
                               Val = "18023",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, IN ",
                               Val = "18025",
                           },
                           new GeoFips
                           {
                               Description = "4 Daviess, IN ",
                               Val = "18027",
                           },
                           new GeoFips
                           {
                               Description = "4 Dearborn, IN ",
                               Val = "18029",
                           },
                           new GeoFips
                           {
                               Description = "4 Decatur, IN ",
                               Val = "18031",
                           },
                           new GeoFips
                           {
                               Description = "4 DeKalb, IN ",
                               Val = "18033",
                           },
                           new GeoFips
                           {
                               Description = "4 Delaware, IN ",
                               Val = "18035",
                           },
                           new GeoFips
                           {
                               Description = "4 Dubois, IN ",
                               Val = "18037",
                           },
                           new GeoFips
                           {
                               Description = "4 Elkhart, IN ",
                               Val = "18039",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, IN ",
                               Val = "18041",
                           },
                           new GeoFips
                           {
                               Description = "4 Floyd, IN ",
                               Val = "18043",
                           },
                           new GeoFips
                           {
                               Description = "4 Fountain, IN ",
                               Val = "18045",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, IN ",
                               Val = "18047",
                           },
                           new GeoFips
                           {
                               Description = "4 Fulton, IN ",
                               Val = "18049",
                           },
                           new GeoFips
                           {
                               Description = "4 Gibson, IN ",
                               Val = "18051",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, IN ",
                               Val = "18053",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, IN ",
                               Val = "18055",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, IN ",
                               Val = "18057",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, IN ",
                               Val = "18059",
                           },
                           new GeoFips
                           {
                               Description = "4 Harrison, IN ",
                               Val = "18061",
                           },
                           new GeoFips
                           {
                               Description = "4 Hendricks, IN ",
                               Val = "18063",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry, IN ",
                               Val = "18065",
                           },
                           new GeoFips
                           {
                               Description = "4 Howard, IN ",
                               Val = "18067",
                           },
                           new GeoFips
                           {
                               Description = "4 Huntington, IN ",
                               Val = "18069",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, IN ",
                               Val = "18071",
                           },
                           new GeoFips
                           {
                               Description = "4 Jasper, IN ",
                               Val = "18073",
                           },
                           new GeoFips
                           {
                               Description = "4 Jay, IN ",
                               Val = "18075",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, IN ",
                               Val = "18077",
                           },
                           new GeoFips
                           {
                               Description = "4 Jennings, IN ",
                               Val = "18079",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, IN ",
                               Val = "18081",
                           },
                           new GeoFips
                           {
                               Description = "4 Knox, IN ",
                               Val = "18083",
                           },
                           new GeoFips
                           {
                               Description = "4 Kosciusko, IN ",
                               Val = "18085",
                           },
                           new GeoFips
                           {
                               Description = "4 Lagrange, IN ",
                               Val = "18087",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, IN ",
                               Val = "18089",
                           },
                           new GeoFips
                           {
                               Description = "4 LaPorte, IN ",
                               Val = "18091",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, IN ",
                               Val = "18093",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, IN ",
                               Val = "18095",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, IN ",
                               Val = "18097",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, IN ",
                               Val = "18099",
                           },
                           new GeoFips
                           {
                               Description = "4 Martin, IN ",
                               Val = "18101",
                           },
                           new GeoFips
                           {
                               Description = "4 Miami, IN ",
                               Val = "18103",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, IN ",
                               Val = "18105",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, IN ",
                               Val = "18107",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, IN ",
                               Val = "18109",
                           },
                           new GeoFips
                           {
                               Description = "4 Newton, IN ",
                               Val = "18111",
                           },
                           new GeoFips
                           {
                               Description = "4 Noble, IN ",
                               Val = "18113",
                           },
                           new GeoFips
                           {
                               Description = "4 Ohio, IN ",
                               Val = "18115",
                           },
                           new GeoFips
                           {
                               Description = "4 Orange, IN ",
                               Val = "18117",
                           },
                           new GeoFips
                           {
                               Description = "4 Owen, IN ",
                               Val = "18119",
                           },
                           new GeoFips
                           {
                               Description = "4 Parke, IN ",
                               Val = "18121",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, IN ",
                               Val = "18123",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, IN ",
                               Val = "18125",
                           },
                           new GeoFips
                           {
                               Description = "4 Porter, IN ",
                               Val = "18127",
                           },
                           new GeoFips
                           {
                               Description = "4 Posey, IN ",
                               Val = "18129",
                           },
                           new GeoFips
                           {
                               Description = "4 Pulaski, IN ",
                               Val = "18131",
                           },
                           new GeoFips
                           {
                               Description = "4 Putnam, IN ",
                               Val = "18133",
                           },
                           new GeoFips
                           {
                               Description = "4 Randolph, IN ",
                               Val = "18135",
                           },
                           new GeoFips
                           {
                               Description = "4 Ripley, IN ",
                               Val = "18137",
                           },
                           new GeoFips
                           {
                               Description = "4 Rush, IN ",
                               Val = "18139",
                           },
                           new GeoFips
                           {
                               Description = "5 Columbus, OH (Metropolitan Statistical Area)",
                               Val = "18140",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Joseph, IN ",
                               Val = "18141",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, IN ",
                               Val = "18143",
                           },
                           new GeoFips
                           {
                               Description = "4 Shelby, IN ",
                               Val = "18145",
                           },
                           new GeoFips
                           {
                               Description = "4 Spencer, IN ",
                               Val = "18147",
                           },
                           new GeoFips
                           {
                               Description = "4 Starke, IN ",
                               Val = "18149",
                           },
                           new GeoFips
                           {
                               Description = "4 Steuben, IN ",
                               Val = "18151",
                           },
                           new GeoFips
                           {
                               Description = "4 Sullivan, IN ",
                               Val = "18153",
                           },
                           new GeoFips
                           {
                               Description = "4 Switzerland, IN ",
                               Val = "18155",
                           },
                           new GeoFips
                           {
                               Description = "4 Tippecanoe, IN ",
                               Val = "18157",
                           },
                           new GeoFips
                           {
                               Description = "4 Tipton, IN ",
                               Val = "18159",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, IN ",
                               Val = "18161",
                           },
                           new GeoFips
                           {
                               Description = "4 Vanderburgh, IN ",
                               Val = "18163",
                           },
                           new GeoFips
                           {
                               Description = "4 Vermillion, IN ",
                               Val = "18165",
                           },
                           new GeoFips
                           {
                               Description = "4 Vigo, IN ",
                               Val = "18167",
                           },
                           new GeoFips
                           {
                               Description = "4 Wabash, IN ",
                               Val = "18169",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, IN ",
                               Val = "18171",
                           },
                           new GeoFips
                           {
                               Description = "4 Warrick, IN ",
                               Val = "18173",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, IN ",
                               Val = "18175",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, IN ",
                               Val = "18177",
                           },
                           new GeoFips
                           {
                               Description = "4 Wells, IN ",
                               Val = "18179",
                           },
                           new GeoFips
                           {
                               Description = "4 White, IN ",
                               Val = "18181",
                           },
                           new GeoFips
                           {
                               Description = "4 Whitley, IN ",
                               Val = "18183",
                           },
                           new GeoFips
                           {
                               Description = "5 Corpus Christi, TX (Metropolitan Statistical Area)",
                               Val = "18580",
                           },
                           new GeoFips
                           {
                               Description = "5 Corvallis, OR (Metropolitan Statistical Area)",
                               Val = "18700",
                           },
                           new GeoFips
                           {
                               Description = "5 Crestview-Fort Walton Beach-Destin, FL (Metropolitan Statistical Area)",
                               Val = "18880",
                           },
                           new GeoFips
                           {
                               Description = "3 Iowa",
                               Val = "19000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adair, IA ",
                               Val = "19001",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, IA ",
                               Val = "19003",
                           },
                           new GeoFips
                           {
                               Description = "4 Allamakee, IA ",
                               Val = "19005",
                           },
                           new GeoFips
                           {
                               Description = "4 Appanoose, IA ",
                               Val = "19007",
                           },
                           new GeoFips
                           {
                               Description = "4 Audubon, IA ",
                               Val = "19009",
                           },
                           new GeoFips
                           {
                               Description = "4 Benton, IA ",
                               Val = "19011",
                           },
                           new GeoFips
                           {
                               Description = "4 Black Hawk, IA ",
                               Val = "19013",
                           },
                           new GeoFips
                           {
                               Description = "4 Boone, IA ",
                               Val = "19015",
                           },
                           new GeoFips
                           {
                               Description = "4 Bremer, IA ",
                               Val = "19017",
                           },
                           new GeoFips
                           {
                               Description = "4 Buchanan, IA ",
                               Val = "19019",
                           },
                           new GeoFips
                           {
                               Description = "4 Buena Vista, IA ",
                               Val = "19021",
                           },
                           new GeoFips
                           {
                               Description = "4 Butler, IA ",
                               Val = "19023",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, IA ",
                               Val = "19025",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, IA ",
                               Val = "19027",
                           },
                           new GeoFips
                           {
                               Description = "4 Cass, IA ",
                               Val = "19029",
                           },
                           new GeoFips
                           {
                               Description = "4 Cedar, IA ",
                               Val = "19031",
                           },
                           new GeoFips
                           {
                               Description = "4 Cerro Gordo, IA ",
                               Val = "19033",
                           },
                           new GeoFips
                           {
                               Description = "4 Cherokee, IA ",
                               Val = "19035",
                           },
                           new GeoFips
                           {
                               Description = "4 Chickasaw, IA ",
                               Val = "19037",
                           },
                           new GeoFips
                           {
                               Description = "4 Clarke, IA ",
                               Val = "19039",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, IA ",
                               Val = "19041",
                           },
                           new GeoFips
                           {
                               Description = "4 Clayton, IA ",
                               Val = "19043",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinton, IA ",
                               Val = "19045",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, IA ",
                               Val = "19047",
                           },
                           new GeoFips
                           {
                               Description = "4 Dallas, IA ",
                               Val = "19049",
                           },
                           new GeoFips
                           {
                               Description = "4 Davis, IA ",
                               Val = "19051",
                           },
                           new GeoFips
                           {
                               Description = "4 Decatur, IA ",
                               Val = "19053",
                           },
                           new GeoFips
                           {
                               Description = "4 Delaware, IA ",
                               Val = "19055",
                           },
                           new GeoFips
                           {
                               Description = "4 Des Moines, IA ",
                               Val = "19057",
                           },
                           new GeoFips
                           {
                               Description = "4 Dickinson, IA ",
                               Val = "19059",
                           },
                           new GeoFips
                           {
                               Description = "5 Cumberland, MD-WV (Metropolitan Statistical Area)",
                               Val = "19060",
                           },
                           new GeoFips
                           {
                               Description = "4 Dubuque, IA ",
                               Val = "19061",
                           },
                           new GeoFips
                           {
                               Description = "4 Emmet, IA ",
                               Val = "19063",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, IA ",
                               Val = "19065",
                           },
                           new GeoFips
                           {
                               Description = "4 Floyd, IA ",
                               Val = "19067",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, IA ",
                               Val = "19069",
                           },
                           new GeoFips
                           {
                               Description = "4 Fremont, IA ",
                               Val = "19071",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, IA ",
                               Val = "19073",
                           },
                           new GeoFips
                           {
                               Description = "4 Grundy, IA ",
                               Val = "19075",
                           },
                           new GeoFips
                           {
                               Description = "4 Guthrie, IA ",
                               Val = "19077",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, IA ",
                               Val = "19079",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, IA ",
                               Val = "19081",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardin, IA ",
                               Val = "19083",
                           },
                           new GeoFips
                           {
                               Description = "4 Harrison, IA ",
                               Val = "19085",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry, IA ",
                               Val = "19087",
                           },
                           new GeoFips
                           {
                               Description = "4 Howard, IA ",
                               Val = "19089",
                           },
                           new GeoFips
                           {
                               Description = "4 Humboldt, IA ",
                               Val = "19091",
                           },
                           new GeoFips
                           {
                               Description = "4 Ida, IA ",
                               Val = "19093",
                           },
                           new GeoFips
                           {
                               Description = "4 Iowa, IA ",
                               Val = "19095",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, IA ",
                               Val = "19097",
                           },
                           new GeoFips
                           {
                               Description = "4 Jasper, IA ",
                               Val = "19099",
                           },
                           new GeoFips
                           {
                               Description = "5 Dallas-Fort Worth-Arlington, TX (Metropolitan Statistical Area)",
                               Val = "19100",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, IA ",
                               Val = "19101",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, IA ",
                               Val = "19103",
                           },
                           new GeoFips
                           {
                               Description = "4 Jones, IA ",
                               Val = "19105",
                           },
                           new GeoFips
                           {
                               Description = "4 Keokuk, IA ",
                               Val = "19107",
                           },
                           new GeoFips
                           {
                               Description = "4 Kossuth, IA ",
                               Val = "19109",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, IA ",
                               Val = "19111",
                           },
                           new GeoFips
                           {
                               Description = "4 Linn, IA ",
                               Val = "19113",
                           },
                           new GeoFips
                           {
                               Description = "4 Louisa, IA ",
                               Val = "19115",
                           },
                           new GeoFips
                           {
                               Description = "4 Lucas, IA ",
                               Val = "19117",
                           },
                           new GeoFips
                           {
                               Description = "4 Lyon, IA ",
                               Val = "19119",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, IA ",
                               Val = "19121",
                           },
                           new GeoFips
                           {
                               Description = "4 Mahaska, IA ",
                               Val = "19123",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, IA ",
                               Val = "19125",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, IA ",
                               Val = "19127",
                           },
                           new GeoFips
                           {
                               Description = "4 Mills, IA ",
                               Val = "19129",
                           },
                           new GeoFips
                           {
                               Description = "4 Mitchell, IA ",
                               Val = "19131",
                           },
                           new GeoFips
                           {
                               Description = "4 Monona, IA ",
                               Val = "19133",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, IA ",
                               Val = "19135",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, IA ",
                               Val = "19137",
                           },
                           new GeoFips
                           {
                               Description = "4 Muscatine, IA ",
                               Val = "19139",
                           },
                           new GeoFips
                           {
                               Description = "5 Dalton, GA (Metropolitan Statistical Area)",
                               Val = "19140",
                           },
                           new GeoFips
                           {
                               Description = "4 O'Brien, IA ",
                               Val = "19141",
                           },
                           new GeoFips
                           {
                               Description = "4 Osceola, IA ",
                               Val = "19143",
                           },
                           new GeoFips
                           {
                               Description = "4 Page, IA ",
                               Val = "19145",
                           },
                           new GeoFips
                           {
                               Description = "4 Palo Alto, IA ",
                               Val = "19147",
                           },
                           new GeoFips
                           {
                               Description = "4 Plymouth, IA ",
                               Val = "19149",
                           },
                           new GeoFips
                           {
                               Description = "4 Pocahontas, IA ",
                               Val = "19151",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, IA ",
                               Val = "19153",
                           },
                           new GeoFips
                           {
                               Description = "4 Pottawattamie, IA ",
                               Val = "19155",
                           },
                           new GeoFips
                           {
                               Description = "4 Poweshiek, IA ",
                               Val = "19157",
                           },
                           new GeoFips
                           {
                               Description = "4 Ringgold, IA ",
                               Val = "19159",
                           },
                           new GeoFips
                           {
                               Description = "4 Sac, IA ",
                               Val = "19161",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, IA ",
                               Val = "19163",
                           },
                           new GeoFips
                           {
                               Description = "4 Shelby, IA ",
                               Val = "19165",
                           },
                           new GeoFips
                           {
                               Description = "4 Sioux, IA ",
                               Val = "19167",
                           },
                           new GeoFips
                           {
                               Description = "4 Story, IA ",
                               Val = "19169",
                           },
                           new GeoFips
                           {
                               Description = "4 Tama, IA ",
                               Val = "19171",
                           },
                           new GeoFips
                           {
                               Description = "4 Taylor, IA ",
                               Val = "19173",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, IA ",
                               Val = "19175",
                           },
                           new GeoFips
                           {
                               Description = "4 Van Buren, IA ",
                               Val = "19177",
                           },
                           new GeoFips
                           {
                               Description = "4 Wapello, IA ",
                               Val = "19179",
                           },
                           new GeoFips
                           {
                               Description = "5 Danville, IL (Metropolitan Statistical Area)",
                               Val = "19180",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, IA ",
                               Val = "19181",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, IA ",
                               Val = "19183",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, IA ",
                               Val = "19185",
                           },
                           new GeoFips
                           {
                               Description = "4 Webster, IA ",
                               Val = "19187",
                           },
                           new GeoFips
                           {
                               Description = "4 Winnebago, IA ",
                               Val = "19189",
                           },
                           new GeoFips
                           {
                               Description = "4 Winneshiek, IA ",
                               Val = "19191",
                           },
                           new GeoFips
                           {
                               Description = "4 Woodbury, IA ",
                               Val = "19193",
                           },
                           new GeoFips
                           {
                               Description = "4 Worth, IA ",
                               Val = "19195",
                           },
                           new GeoFips
                           {
                               Description = "4 Wright, IA ",
                               Val = "19197",
                           },
                           new GeoFips
                           {
                               Description = "5 Daphne-Fairhope-Foley, AL (Metropolitan Statistical Area)",
                               Val = "19300",
                           },
                           new GeoFips
                           {
                               Description = "5 Davenport-Moline-Rock Island, IA-IL (Metropolitan Statistical Area)",
                               Val = "19340",
                           },
                           new GeoFips
                           {
                               Description = "5 Dayton, OH (Metropolitan Statistical Area)",
                               Val = "19380",
                           },
                           new GeoFips
                           {
                               Description = "5 Decatur, AL (Metropolitan Statistical Area)",
                               Val = "19460",
                           },
                           new GeoFips
                           {
                               Description = "5 Decatur, IL (Metropolitan Statistical Area)",
                               Val = "19500",
                           },
                           new GeoFips
                           {
                               Description = "5 Deltona-Daytona Beach-Ormond Beach, FL (Metropolitan Statistical Area)",
                               Val = "19660",
                           },
                           new GeoFips
                           {
                               Description = "5 Denver-Aurora-Lakewood, CO (Metropolitan Statistical Area)",
                               Val = "19740",
                           },
                           new GeoFips
                           {
                               Description = "5 Des Moines-West Des Moines, IA (Metropolitan Statistical Area)",
                               Val = "19780",
                           },
                           new GeoFips
                           {
                               Description = "5 Detroit-Warren-Dearborn, MI (Metropolitan Statistical Area)",
                               Val = "19820",
                           },
                           new GeoFips
                           {
                               Description = "3 Kansas",
                               Val = "20000",
                           },
                           new GeoFips
                           {
                               Description = "4 Allen, KS ",
                               Val = "20001",
                           },
                           new GeoFips
                           {
                               Description = "4 Anderson, KS ",
                               Val = "20003",
                           },
                           new GeoFips
                           {
                               Description = "4 Atchison, KS ",
                               Val = "20005",
                           },
                           new GeoFips
                           {
                               Description = "4 Barber, KS ",
                               Val = "20007",
                           },
                           new GeoFips
                           {
                               Description = "4 Barton, KS ",
                               Val = "20009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bourbon, KS ",
                               Val = "20011",
                           },
                           new GeoFips
                           {
                               Description = "4 Brown, KS ",
                               Val = "20013",
                           },
                           new GeoFips
                           {
                               Description = "4 Butler, KS ",
                               Val = "20015",
                           },
                           new GeoFips
                           {
                               Description = "4 Chase, KS ",
                               Val = "20017",
                           },
                           new GeoFips
                           {
                               Description = "4 Chautauqua, KS ",
                               Val = "20019",
                           },
                           new GeoFips
                           {
                               Description = "5 Dothan, AL (Metropolitan Statistical Area)",
                               Val = "20020",
                           },
                           new GeoFips
                           {
                               Description = "4 Cherokee, KS ",
                               Val = "20021",
                           },
                           new GeoFips
                           {
                               Description = "4 Cheyenne, KS ",
                               Val = "20023",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, KS ",
                               Val = "20025",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, KS ",
                               Val = "20027",
                           },
                           new GeoFips
                           {
                               Description = "4 Cloud, KS ",
                               Val = "20029",
                           },
                           new GeoFips
                           {
                               Description = "4 Coffey, KS ",
                               Val = "20031",
                           },
                           new GeoFips
                           {
                               Description = "4 Comanche, KS ",
                               Val = "20033",
                           },
                           new GeoFips
                           {
                               Description = "4 Cowley, KS ",
                               Val = "20035",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, KS ",
                               Val = "20037",
                           },
                           new GeoFips
                           {
                               Description = "4 Decatur, KS ",
                               Val = "20039",
                           },
                           new GeoFips
                           {
                               Description = "4 Dickinson, KS ",
                               Val = "20041",
                           },
                           new GeoFips
                           {
                               Description = "4 Doniphan, KS ",
                               Val = "20043",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, KS ",
                               Val = "20045",
                           },
                           new GeoFips
                           {
                               Description = "4 Edwards, KS ",
                               Val = "20047",
                           },
                           new GeoFips
                           {
                               Description = "4 Elk, KS ",
                               Val = "20049",
                           },
                           new GeoFips
                           {
                               Description = "4 Ellis, KS ",
                               Val = "20051",
                           },
                           new GeoFips
                           {
                               Description = "4 Ellsworth, KS ",
                               Val = "20053",
                           },
                           new GeoFips
                           {
                               Description = "4 Finney, KS ",
                               Val = "20055",
                           },
                           new GeoFips
                           {
                               Description = "4 Ford, KS ",
                               Val = "20057",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, KS ",
                               Val = "20059",
                           },
                           new GeoFips
                           {
                               Description = "4 Geary, KS ",
                               Val = "20061",
                           },
                           new GeoFips
                           {
                               Description = "4 Gove, KS ",
                               Val = "20063",
                           },
                           new GeoFips
                           {
                               Description = "4 Graham, KS ",
                               Val = "20065",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, KS ",
                               Val = "20067",
                           },
                           new GeoFips
                           {
                               Description = "4 Gray, KS ",
                               Val = "20069",
                           },
                           new GeoFips
                           {
                               Description = "4 Greeley, KS ",
                               Val = "20071",
                           },
                           new GeoFips
                           {
                               Description = "4 Greenwood, KS ",
                               Val = "20073",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, KS ",
                               Val = "20075",
                           },
                           new GeoFips
                           {
                               Description = "4 Harper, KS ",
                               Val = "20077",
                           },
                           new GeoFips
                           {
                               Description = "4 Harvey, KS ",
                               Val = "20079",
                           },
                           new GeoFips
                           {
                               Description = "4 Haskell, KS ",
                               Val = "20081",
                           },
                           new GeoFips
                           {
                               Description = "4 Hodgeman, KS ",
                               Val = "20083",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, KS ",
                               Val = "20085",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, KS ",
                               Val = "20087",
                           },
                           new GeoFips
                           {
                               Description = "4 Jewell, KS ",
                               Val = "20089",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, KS ",
                               Val = "20091",
                           },
                           new GeoFips
                           {
                               Description = "4 Kearny, KS ",
                               Val = "20093",
                           },
                           new GeoFips
                           {
                               Description = "4 Kingman, KS ",
                               Val = "20095",
                           },
                           new GeoFips
                           {
                               Description = "4 Kiowa, KS ",
                               Val = "20097",
                           },
                           new GeoFips
                           {
                               Description = "4 Labette, KS ",
                               Val = "20099",
                           },
                           new GeoFips
                           {
                               Description = "5 Dover, DE (Metropolitan Statistical Area)",
                               Val = "20100",
                           },
                           new GeoFips
                           {
                               Description = "4 Lane, KS ",
                               Val = "20101",
                           },
                           new GeoFips
                           {
                               Description = "4 Leavenworth, KS ",
                               Val = "20103",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, KS ",
                               Val = "20105",
                           },
                           new GeoFips
                           {
                               Description = "4 Linn, KS ",
                               Val = "20107",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, KS ",
                               Val = "20109",
                           },
                           new GeoFips
                           {
                               Description = "4 Lyon, KS ",
                               Val = "20111",
                           },
                           new GeoFips
                           {
                               Description = "4 McPherson, KS ",
                               Val = "20113",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, KS ",
                               Val = "20115",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, KS ",
                               Val = "20117",
                           },
                           new GeoFips
                           {
                               Description = "4 Meade, KS ",
                               Val = "20119",
                           },
                           new GeoFips
                           {
                               Description = "4 Miami, KS ",
                               Val = "20121",
                           },
                           new GeoFips
                           {
                               Description = "4 Mitchell, KS ",
                               Val = "20123",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, KS ",
                               Val = "20125",
                           },
                           new GeoFips
                           {
                               Description = "4 Morris, KS ",
                               Val = "20127",
                           },
                           new GeoFips
                           {
                               Description = "4 Morton, KS ",
                               Val = "20129",
                           },
                           new GeoFips
                           {
                               Description = "4 Nemaha, KS ",
                               Val = "20131",
                           },
                           new GeoFips
                           {
                               Description = "4 Neosho, KS ",
                               Val = "20133",
                           },
                           new GeoFips
                           {
                               Description = "4 Ness, KS ",
                               Val = "20135",
                           },
                           new GeoFips
                           {
                               Description = "4 Norton, KS ",
                               Val = "20137",
                           },
                           new GeoFips
                           {
                               Description = "4 Osage, KS ",
                               Val = "20139",
                           },
                           new GeoFips
                           {
                               Description = "4 Osborne, KS ",
                               Val = "20141",
                           },
                           new GeoFips
                           {
                               Description = "4 Ottawa, KS ",
                               Val = "20143",
                           },
                           new GeoFips
                           {
                               Description = "4 Pawnee, KS ",
                               Val = "20145",
                           },
                           new GeoFips
                           {
                               Description = "4 Phillips, KS ",
                               Val = "20147",
                           },
                           new GeoFips
                           {
                               Description = "4 Pottawatomie, KS ",
                               Val = "20149",
                           },
                           new GeoFips
                           {
                               Description = "4 Pratt, KS ",
                               Val = "20151",
                           },
                           new GeoFips
                           {
                               Description = "4 Rawlins, KS ",
                               Val = "20153",
                           },
                           new GeoFips
                           {
                               Description = "4 Reno, KS ",
                               Val = "20155",
                           },
                           new GeoFips
                           {
                               Description = "4 Republic, KS ",
                               Val = "20157",
                           },
                           new GeoFips
                           {
                               Description = "4 Rice, KS ",
                               Val = "20159",
                           },
                           new GeoFips
                           {
                               Description = "4 Riley, KS ",
                               Val = "20161",
                           },
                           new GeoFips
                           {
                               Description = "4 Rooks, KS ",
                               Val = "20163",
                           },
                           new GeoFips
                           {
                               Description = "4 Rush, KS ",
                               Val = "20165",
                           },
                           new GeoFips
                           {
                               Description = "4 Russell, KS ",
                               Val = "20167",
                           },
                           new GeoFips
                           {
                               Description = "4 Saline, KS ",
                               Val = "20169",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, KS ",
                               Val = "20171",
                           },
                           new GeoFips
                           {
                               Description = "4 Sedgwick, KS ",
                               Val = "20173",
                           },
                           new GeoFips
                           {
                               Description = "4 Seward, KS ",
                               Val = "20175",
                           },
                           new GeoFips
                           {
                               Description = "4 Shawnee, KS ",
                               Val = "20177",
                           },
                           new GeoFips
                           {
                               Description = "4 Sheridan, KS ",
                               Val = "20179",
                           },
                           new GeoFips
                           {
                               Description = "4 Sherman, KS ",
                               Val = "20181",
                           },
                           new GeoFips
                           {
                               Description = "4 Smith, KS ",
                               Val = "20183",
                           },
                           new GeoFips
                           {
                               Description = "4 Stafford, KS ",
                               Val = "20185",
                           },
                           new GeoFips
                           {
                               Description = "4 Stanton, KS ",
                               Val = "20187",
                           },
                           new GeoFips
                           {
                               Description = "4 Stevens, KS ",
                               Val = "20189",
                           },
                           new GeoFips
                           {
                               Description = "4 Sumner, KS ",
                               Val = "20191",
                           },
                           new GeoFips
                           {
                               Description = "4 Thomas, KS ",
                               Val = "20193",
                           },
                           new GeoFips
                           {
                               Description = "4 Trego, KS ",
                               Val = "20195",
                           },
                           new GeoFips
                           {
                               Description = "4 Wabaunsee, KS ",
                               Val = "20197",
                           },
                           new GeoFips
                           {
                               Description = "4 Wallace, KS ",
                               Val = "20199",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, KS ",
                               Val = "20201",
                           },
                           new GeoFips
                           {
                               Description = "4 Wichita, KS ",
                               Val = "20203",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilson, KS ",
                               Val = "20205",
                           },
                           new GeoFips
                           {
                               Description = "4 Woodson, KS ",
                               Val = "20207",
                           },
                           new GeoFips
                           {
                               Description = "4 Wyandotte, KS ",
                               Val = "20209",
                           },
                           new GeoFips
                           {
                               Description = "5 Dubuque, IA (Metropolitan Statistical Area)",
                               Val = "20220",
                           },
                           new GeoFips
                           {
                               Description = "5 Duluth, MN-WI (Metropolitan Statistical Area)",
                               Val = "20260",
                           },
                           new GeoFips
                           {
                               Description = "5 Durham-Chapel Hill, NC (Metropolitan Statistical Area)",
                               Val = "20500",
                           },
                           new GeoFips
                           {
                               Description = "5 East Stroudsburg, PA (Metropolitan Statistical Area)",
                               Val = "20700",
                           },
                           new GeoFips
                           {
                               Description = "5 Eau Claire, WI (Metropolitan Statistical Area)",
                               Val = "20740",
                           },
                           new GeoFips
                           {
                               Description = "5 El Centro, CA (Metropolitan Statistical Area)",
                               Val = "20940",
                           },
                           new GeoFips
                           {
                               Description = "3 Kentucky",
                               Val = "21000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adair, KY ",
                               Val = "21001",
                           },
                           new GeoFips
                           {
                               Description = "4 Allen, KY ",
                               Val = "21003",
                           },
                           new GeoFips
                           {
                               Description = "4 Anderson, KY ",
                               Val = "21005",
                           },
                           new GeoFips
                           {
                               Description = "4 Ballard, KY ",
                               Val = "21007",
                           },
                           new GeoFips
                           {
                               Description = "4 Barren, KY ",
                               Val = "21009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bath, KY ",
                               Val = "21011",
                           },
                           new GeoFips
                           {
                               Description = "4 Bell, KY ",
                               Val = "21013",
                           },
                           new GeoFips
                           {
                               Description = "4 Boone, KY ",
                               Val = "21015",
                           },
                           new GeoFips
                           {
                               Description = "4 Bourbon, KY ",
                               Val = "21017",
                           },
                           new GeoFips
                           {
                               Description = "4 Boyd, KY ",
                               Val = "21019",
                           },
                           new GeoFips
                           {
                               Description = "4 Boyle, KY ",
                               Val = "21021",
                           },
                           new GeoFips
                           {
                               Description = "4 Bracken, KY ",
                               Val = "21023",
                           },
                           new GeoFips
                           {
                               Description = "4 Breathitt, KY ",
                               Val = "21025",
                           },
                           new GeoFips
                           {
                               Description = "4 Breckinridge, KY ",
                               Val = "21027",
                           },
                           new GeoFips
                           {
                               Description = "4 Bullitt, KY ",
                               Val = "21029",
                           },
                           new GeoFips
                           {
                               Description = "4 Butler, KY ",
                               Val = "21031",
                           },
                           new GeoFips
                           {
                               Description = "4 Caldwell, KY ",
                               Val = "21033",
                           },
                           new GeoFips
                           {
                               Description = "4 Calloway, KY ",
                               Val = "21035",
                           },
                           new GeoFips
                           {
                               Description = "4 Campbell, KY ",
                               Val = "21037",
                           },
                           new GeoFips
                           {
                               Description = "4 Carlisle, KY ",
                               Val = "21039",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, KY ",
                               Val = "21041",
                           },
                           new GeoFips
                           {
                               Description = "4 Carter, KY ",
                               Val = "21043",
                           },
                           new GeoFips
                           {
                               Description = "4 Casey, KY ",
                               Val = "21045",
                           },
                           new GeoFips
                           {
                               Description = "4 Christian, KY ",
                               Val = "21047",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, KY ",
                               Val = "21049",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, KY ",
                               Val = "21051",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinton, KY ",
                               Val = "21053",
                           },
                           new GeoFips
                           {
                               Description = "4 Crittenden, KY ",
                               Val = "21055",
                           },
                           new GeoFips
                           {
                               Description = "4 Cumberland, KY ",
                               Val = "21057",
                           },
                           new GeoFips
                           {
                               Description = "4 Daviess, KY ",
                               Val = "21059",
                           },
                           new GeoFips
                           {
                               Description = "5 Elizabethtown-Fort Knox, KY (Metropolitan Statistical Area)",
                               Val = "21060",
                           },
                           new GeoFips
                           {
                               Description = "4 Edmonson, KY ",
                               Val = "21061",
                           },
                           new GeoFips
                           {
                               Description = "4 Elliott, KY ",
                               Val = "21063",
                           },
                           new GeoFips
                           {
                               Description = "4 Estill, KY ",
                               Val = "21065",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, KY ",
                               Val = "21067",
                           },
                           new GeoFips
                           {
                               Description = "4 Fleming, KY ",
                               Val = "21069",
                           },
                           new GeoFips
                           {
                               Description = "4 Floyd, KY ",
                               Val = "21071",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, KY ",
                               Val = "21073",
                           },
                           new GeoFips
                           {
                               Description = "4 Fulton, KY ",
                               Val = "21075",
                           },
                           new GeoFips
                           {
                               Description = "4 Gallatin, KY ",
                               Val = "21077",
                           },
                           new GeoFips
                           {
                               Description = "4 Garrard, KY ",
                               Val = "21079",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, KY ",
                               Val = "21081",
                           },
                           new GeoFips
                           {
                               Description = "4 Graves, KY ",
                               Val = "21083",
                           },
                           new GeoFips
                           {
                               Description = "4 Grayson, KY ",
                               Val = "21085",
                           },
                           new GeoFips
                           {
                               Description = "4 Green, KY ",
                               Val = "21087",
                           },
                           new GeoFips
                           {
                               Description = "4 Greenup, KY ",
                               Val = "21089",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, KY ",
                               Val = "21091",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardin, KY ",
                               Val = "21093",
                           },
                           new GeoFips
                           {
                               Description = "4 Harlan, KY ",
                               Val = "21095",
                           },
                           new GeoFips
                           {
                               Description = "4 Harrison, KY ",
                               Val = "21097",
                           },
                           new GeoFips
                           {
                               Description = "4 Hart, KY ",
                               Val = "21099",
                           },
                           new GeoFips
                           {
                               Description = "4 Henderson, KY ",
                               Val = "21101",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry, KY ",
                               Val = "21103",
                           },
                           new GeoFips
                           {
                               Description = "4 Hickman, KY ",
                               Val = "21105",
                           },
                           new GeoFips
                           {
                               Description = "4 Hopkins, KY ",
                               Val = "21107",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, KY ",
                               Val = "21109",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, KY ",
                               Val = "21111",
                           },
                           new GeoFips
                           {
                               Description = "4 Jessamine, KY ",
                               Val = "21113",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, KY ",
                               Val = "21115",
                           },
                           new GeoFips
                           {
                               Description = "4 Kenton, KY ",
                               Val = "21117",
                           },
                           new GeoFips
                           {
                               Description = "4 Knott, KY ",
                               Val = "21119",
                           },
                           new GeoFips
                           {
                               Description = "4 Knox, KY ",
                               Val = "21121",
                           },
                           new GeoFips
                           {
                               Description = "4 Larue, KY ",
                               Val = "21123",
                           },
                           new GeoFips
                           {
                               Description = "4 Laurel, KY ",
                               Val = "21125",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, KY ",
                               Val = "21127",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, KY ",
                               Val = "21129",
                           },
                           new GeoFips
                           {
                               Description = "4 Leslie, KY ",
                               Val = "21131",
                           },
                           new GeoFips
                           {
                               Description = "4 Letcher, KY ",
                               Val = "21133",
                           },
                           new GeoFips
                           {
                               Description = "4 Lewis, KY ",
                               Val = "21135",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, KY ",
                               Val = "21137",
                           },
                           new GeoFips
                           {
                               Description = "4 Livingston, KY ",
                               Val = "21139",
                           },
                           new GeoFips
                           {
                               Description = "5 Elkhart-Goshen, IN (Metropolitan Statistical Area)",
                               Val = "21140",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, KY ",
                               Val = "21141",
                           },
                           new GeoFips
                           {
                               Description = "4 Lyon, KY ",
                               Val = "21143",
                           },
                           new GeoFips
                           {
                               Description = "4 McCracken, KY ",
                               Val = "21145",
                           },
                           new GeoFips
                           {
                               Description = "4 McCreary, KY ",
                               Val = "21147",
                           },
                           new GeoFips
                           {
                               Description = "4 McLean, KY ",
                               Val = "21149",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, KY ",
                               Val = "21151",
                           },
                           new GeoFips
                           {
                               Description = "4 Magoffin, KY ",
                               Val = "21153",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, KY ",
                               Val = "21155",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, KY ",
                               Val = "21157",
                           },
                           new GeoFips
                           {
                               Description = "4 Martin, KY ",
                               Val = "21159",
                           },
                           new GeoFips
                           {
                               Description = "4 Mason, KY ",
                               Val = "21161",
                           },
                           new GeoFips
                           {
                               Description = "4 Meade, KY ",
                               Val = "21163",
                           },
                           new GeoFips
                           {
                               Description = "4 Menifee, KY ",
                               Val = "21165",
                           },
                           new GeoFips
                           {
                               Description = "4 Mercer, KY ",
                               Val = "21167",
                           },
                           new GeoFips
                           {
                               Description = "4 Metcalfe, KY ",
                               Val = "21169",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, KY ",
                               Val = "21171",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, KY ",
                               Val = "21173",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, KY ",
                               Val = "21175",
                           },
                           new GeoFips
                           {
                               Description = "4 Muhlenberg, KY ",
                               Val = "21177",
                           },
                           new GeoFips
                           {
                               Description = "4 Nelson, KY ",
                               Val = "21179",
                           },
                           new GeoFips
                           {
                               Description = "4 Nicholas, KY ",
                               Val = "21181",
                           },
                           new GeoFips
                           {
                               Description = "4 Ohio, KY ",
                               Val = "21183",
                           },
                           new GeoFips
                           {
                               Description = "4 Oldham, KY ",
                               Val = "21185",
                           },
                           new GeoFips
                           {
                               Description = "4 Owen, KY ",
                               Val = "21187",
                           },
                           new GeoFips
                           {
                               Description = "4 Owsley, KY ",
                               Val = "21189",
                           },
                           new GeoFips
                           {
                               Description = "4 Pendleton, KY ",
                               Val = "21191",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, KY ",
                               Val = "21193",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, KY ",
                               Val = "21195",
                           },
                           new GeoFips
                           {
                               Description = "4 Powell, KY ",
                               Val = "21197",
                           },
                           new GeoFips
                           {
                               Description = "4 Pulaski, KY ",
                               Val = "21199",
                           },
                           new GeoFips
                           {
                               Description = "4 Robertson, KY ",
                               Val = "21201",
                           },
                           new GeoFips
                           {
                               Description = "4 Rockcastle, KY ",
                               Val = "21203",
                           },
                           new GeoFips
                           {
                               Description = "4 Rowan, KY ",
                               Val = "21205",
                           },
                           new GeoFips
                           {
                               Description = "4 Russell, KY ",
                               Val = "21207",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, KY ",
                               Val = "21209",
                           },
                           new GeoFips
                           {
                               Description = "4 Shelby, KY ",
                               Val = "21211",
                           },
                           new GeoFips
                           {
                               Description = "4 Simpson, KY ",
                               Val = "21213",
                           },
                           new GeoFips
                           {
                               Description = "4 Spencer, KY ",
                               Val = "21215",
                           },
                           new GeoFips
                           {
                               Description = "4 Taylor, KY ",
                               Val = "21217",
                           },
                           new GeoFips
                           {
                               Description = "4 Todd, KY ",
                               Val = "21219",
                           },
                           new GeoFips
                           {
                               Description = "4 Trigg, KY ",
                               Val = "21221",
                           },
                           new GeoFips
                           {
                               Description = "4 Trimble, KY ",
                               Val = "21223",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, KY ",
                               Val = "21225",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, KY ",
                               Val = "21227",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, KY ",
                               Val = "21229",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, KY ",
                               Val = "21231",
                           },
                           new GeoFips
                           {
                               Description = "4 Webster, KY ",
                               Val = "21233",
                           },
                           new GeoFips
                           {
                               Description = "4 Whitley, KY ",
                               Val = "21235",
                           },
                           new GeoFips
                           {
                               Description = "4 Wolfe, KY ",
                               Val = "21237",
                           },
                           new GeoFips
                           {
                               Description = "4 Woodford, KY ",
                               Val = "21239",
                           },
                           new GeoFips
                           {
                               Description = "5 Elmira, NY (Metropolitan Statistical Area)",
                               Val = "21300",
                           },
                           new GeoFips
                           {
                               Description = "5 El Paso, TX (Metropolitan Statistical Area)",
                               Val = "21340",
                           },
                           new GeoFips
                           {
                               Description = "5 Erie, PA (Metropolitan Statistical Area)",
                               Val = "21500",
                           },
                           new GeoFips
                           {
                               Description = "5 Eugene, OR (Metropolitan Statistical Area)",
                               Val = "21660",
                           },
                           new GeoFips
                           {
                               Description = "5 Evansville, IN-KY (Metropolitan Statistical Area)",
                               Val = "21780",
                           },
                           new GeoFips
                           {
                               Description = "5 Fairbanks, AK (Metropolitan Statistical Area)",
                               Val = "21820",
                           },
                           new GeoFips
                           {
                               Description = "3 Louisiana",
                               Val = "22000",
                           },
                           new GeoFips
                           {
                               Description = "4 Acadia, LA ",
                               Val = "22001",
                           },
                           new GeoFips
                           {
                               Description = "4 Allen, LA ",
                               Val = "22003",
                           },
                           new GeoFips
                           {
                               Description = "4 Ascension, LA ",
                               Val = "22005",
                           },
                           new GeoFips
                           {
                               Description = "4 Assumption, LA ",
                               Val = "22007",
                           },
                           new GeoFips
                           {
                               Description = "4 Avoyelles, LA ",
                               Val = "22009",
                           },
                           new GeoFips
                           {
                               Description = "4 Beauregard, LA ",
                               Val = "22011",
                           },
                           new GeoFips
                           {
                               Description = "4 Bienville, LA ",
                               Val = "22013",
                           },
                           new GeoFips
                           {
                               Description = "4 Bossier, LA ",
                               Val = "22015",
                           },
                           new GeoFips
                           {
                               Description = "4 Caddo, LA ",
                               Val = "22017",
                           },
                           new GeoFips
                           {
                               Description = "4 Calcasieu, LA ",
                               Val = "22019",
                           },
                           new GeoFips
                           {
                               Description = "5 Fargo, ND-MN (Metropolitan Statistical Area)",
                               Val = "22020",
                           },
                           new GeoFips
                           {
                               Description = "4 Caldwell, LA ",
                               Val = "22021",
                           },
                           new GeoFips
                           {
                               Description = "4 Cameron, LA ",
                               Val = "22023",
                           },
                           new GeoFips
                           {
                               Description = "4 Catahoula, LA ",
                               Val = "22025",
                           },
                           new GeoFips
                           {
                               Description = "4 Claiborne, LA ",
                               Val = "22027",
                           },
                           new GeoFips
                           {
                               Description = "4 Concordia, LA ",
                               Val = "22029",
                           },
                           new GeoFips
                           {
                               Description = "4 De Soto, LA ",
                               Val = "22031",
                           },
                           new GeoFips
                           {
                               Description = "4 East Baton Rouge, LA ",
                               Val = "22033",
                           },
                           new GeoFips
                           {
                               Description = "4 East Carroll, LA ",
                               Val = "22035",
                           },
                           new GeoFips
                           {
                               Description = "4 East Feliciana, LA ",
                               Val = "22037",
                           },
                           new GeoFips
                           {
                               Description = "4 Evangeline, LA ",
                               Val = "22039",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, LA ",
                               Val = "22041",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, LA ",
                               Val = "22043",
                           },
                           new GeoFips
                           {
                               Description = "4 Iberia, LA ",
                               Val = "22045",
                           },
                           new GeoFips
                           {
                               Description = "4 Iberville, LA ",
                               Val = "22047",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, LA ",
                               Val = "22049",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, LA ",
                               Val = "22051",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson Davis, LA ",
                               Val = "22053",
                           },
                           new GeoFips
                           {
                               Description = "4 Lafayette, LA ",
                               Val = "22055",
                           },
                           new GeoFips
                           {
                               Description = "4 Lafourche, LA ",
                               Val = "22057",
                           },
                           new GeoFips
                           {
                               Description = "4 La Salle, LA ",
                               Val = "22059",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, LA ",
                               Val = "22061",
                           },
                           new GeoFips
                           {
                               Description = "4 Livingston, LA ",
                               Val = "22063",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, LA ",
                               Val = "22065",
                           },
                           new GeoFips
                           {
                               Description = "4 Morehouse, LA ",
                               Val = "22067",
                           },
                           new GeoFips
                           {
                               Description = "4 Natchitoches, LA ",
                               Val = "22069",
                           },
                           new GeoFips
                           {
                               Description = "4 Orleans, LA ",
                               Val = "22071",
                           },
                           new GeoFips
                           {
                               Description = "4 Ouachita, LA ",
                               Val = "22073",
                           },
                           new GeoFips
                           {
                               Description = "4 Plaquemines, LA ",
                               Val = "22075",
                           },
                           new GeoFips
                           {
                               Description = "4 Pointe Coupee, LA ",
                               Val = "22077",
                           },
                           new GeoFips
                           {
                               Description = "4 Rapides, LA ",
                               Val = "22079",
                           },
                           new GeoFips
                           {
                               Description = "4 Red River, LA ",
                               Val = "22081",
                           },
                           new GeoFips
                           {
                               Description = "4 Richland, LA ",
                               Val = "22083",
                           },
                           new GeoFips
                           {
                               Description = "4 Sabine, LA ",
                               Val = "22085",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Bernard, LA ",
                               Val = "22087",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Charles, LA ",
                               Val = "22089",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Helena, LA ",
                               Val = "22091",
                           },
                           new GeoFips
                           {
                               Description = "4 St. James, LA ",
                               Val = "22093",
                           },
                           new GeoFips
                           {
                               Description = "4 St. John the Baptist, LA ",
                               Val = "22095",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Landry, LA ",
                               Val = "22097",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Martin, LA ",
                               Val = "22099",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Mary, LA ",
                               Val = "22101",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Tammany, LA ",
                               Val = "22103",
                           },
                           new GeoFips
                           {
                               Description = "4 Tangipahoa, LA ",
                               Val = "22105",
                           },
                           new GeoFips
                           {
                               Description = "4 Tensas, LA ",
                               Val = "22107",
                           },
                           new GeoFips
                           {
                               Description = "4 Terrebonne, LA ",
                               Val = "22109",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, LA ",
                               Val = "22111",
                           },
                           new GeoFips
                           {
                               Description = "4 Vermilion, LA ",
                               Val = "22113",
                           },
                           new GeoFips
                           {
                               Description = "4 Vernon, LA ",
                               Val = "22115",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, LA ",
                               Val = "22117",
                           },
                           new GeoFips
                           {
                               Description = "4 Webster, LA ",
                               Val = "22119",
                           },
                           new GeoFips
                           {
                               Description = "4 West Baton Rouge, LA ",
                               Val = "22121",
                           },
                           new GeoFips
                           {
                               Description = "4 West Carroll, LA ",
                               Val = "22123",
                           },
                           new GeoFips
                           {
                               Description = "4 West Feliciana, LA ",
                               Val = "22125",
                           },
                           new GeoFips
                           {
                               Description = "4 Winn, LA ",
                               Val = "22127",
                           },
                           new GeoFips
                           {
                               Description = "5 Farmington, NM (Metropolitan Statistical Area)",
                               Val = "22140",
                           },
                           new GeoFips
                           {
                               Description = "5 Fayetteville, NC (Metropolitan Statistical Area)",
                               Val = "22180",
                           },
                           new GeoFips
                           {
                               Description = "5 Fayetteville-Springdale-Rogers, AR-MO (Metropolitan Statistical Area)",
                               Val = "22220",
                           },
                           new GeoFips
                           {
                               Description = "5 Flagstaff, AZ (Metropolitan Statistical Area)",
                               Val = "22380",
                           },
                           new GeoFips
                           {
                               Description = "5 Flint, MI (Metropolitan Statistical Area)",
                               Val = "22420",
                           },
                           new GeoFips
                           {
                               Description = "5 Florence, SC (Metropolitan Statistical Area)",
                               Val = "22500",
                           },
                           new GeoFips
                           {
                               Description = "5 Florence-Muscle Shoals, AL (Metropolitan Statistical Area)",
                               Val = "22520",
                           },
                           new GeoFips
                           {
                               Description = "5 Fond du Lac, WI (Metropolitan Statistical Area)",
                               Val = "22540",
                           },
                           new GeoFips
                           {
                               Description = "5 Fort Collins, CO (Metropolitan Statistical Area)",
                               Val = "22660",
                           },
                           new GeoFips
                           {
                               Description = "5 Fort Smith, AR-OK (Metropolitan Statistical Area)",
                               Val = "22900",
                           },
                           new GeoFips
                           {
                               Description = "3 Maine",
                               Val = "23000",
                           },
                           new GeoFips
                           {
                               Description = "4 Androscoggin, ME ",
                               Val = "23001",
                           },
                           new GeoFips
                           {
                               Description = "4 Aroostook, ME ",
                               Val = "23003",
                           },
                           new GeoFips
                           {
                               Description = "4 Cumberland, ME ",
                               Val = "23005",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, ME ",
                               Val = "23007",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, ME ",
                               Val = "23009",
                           },
                           new GeoFips
                           {
                               Description = "4 Kennebec, ME ",
                               Val = "23011",
                           },
                           new GeoFips
                           {
                               Description = "4 Knox, ME ",
                               Val = "23013",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, ME ",
                               Val = "23015",
                           },
                           new GeoFips
                           {
                               Description = "4 Oxford, ME ",
                               Val = "23017",
                           },
                           new GeoFips
                           {
                               Description = "4 Penobscot, ME ",
                               Val = "23019",
                           },
                           new GeoFips
                           {
                               Description = "4 Piscataquis, ME ",
                               Val = "23021",
                           },
                           new GeoFips
                           {
                               Description = "4 Sagadahoc, ME ",
                               Val = "23023",
                           },
                           new GeoFips
                           {
                               Description = "4 Somerset, ME ",
                               Val = "23025",
                           },
                           new GeoFips
                           {
                               Description = "4 Waldo, ME ",
                               Val = "23027",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, ME ",
                               Val = "23029",
                           },
                           new GeoFips
                           {
                               Description = "4 York, ME ",
                               Val = "23031",
                           },
                           new GeoFips
                           {
                               Description = "5 Fort Wayne, IN (Metropolitan Statistical Area)",
                               Val = "23060",
                           },
                           new GeoFips
                           {
                               Description = "5 Fresno, CA (Metropolitan Statistical Area)",
                               Val = "23420",
                           },
                           new GeoFips
                           {
                               Description = "5 Gadsden, AL (Metropolitan Statistical Area)",
                               Val = "23460",
                           },
                           new GeoFips
                           {
                               Description = "5 Gainesville, FL (Metropolitan Statistical Area)",
                               Val = "23540",
                           },
                           new GeoFips
                           {
                               Description = "5 Gainesville, GA (Metropolitan Statistical Area)",
                               Val = "23580",
                           },
                           new GeoFips
                           {
                               Description = "5 Gettysburg, PA (Metropolitan Statistical Area)",
                               Val = "23900",
                           },
                           new GeoFips
                           {
                               Description = "3 Maryland",
                               Val = "24000",
                           },
                           new GeoFips
                           {
                               Description = "4 Allegany, MD ",
                               Val = "24001",
                           },
                           new GeoFips
                           {
                               Description = "4 Anne Arundel, MD ",
                               Val = "24003",
                           },
                           new GeoFips
                           {
                               Description = "4 Baltimore, MD ",
                               Val = "24005",
                           },
                           new GeoFips
                           {
                               Description = "4 Calvert, MD ",
                               Val = "24009",
                           },
                           new GeoFips
                           {
                               Description = "4 Caroline, MD ",
                               Val = "24011",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, MD ",
                               Val = "24013",
                           },
                           new GeoFips
                           {
                               Description = "4 Cecil, MD ",
                               Val = "24015",
                           },
                           new GeoFips
                           {
                               Description = "4 Charles, MD ",
                               Val = "24017",
                           },
                           new GeoFips
                           {
                               Description = "4 Dorchester, MD ",
                               Val = "24019",
                           },
                           new GeoFips
                           {
                               Description = "5 Glens Falls, NY (Metropolitan Statistical Area)",
                               Val = "24020",
                           },
                           new GeoFips
                           {
                               Description = "4 Frederick, MD ",
                               Val = "24021",
                           },
                           new GeoFips
                           {
                               Description = "4 Garrett, MD ",
                               Val = "24023",
                           },
                           new GeoFips
                           {
                               Description = "4 Harford, MD ",
                               Val = "24025",
                           },
                           new GeoFips
                           {
                               Description = "4 Howard, MD ",
                               Val = "24027",
                           },
                           new GeoFips
                           {
                               Description = "4 Kent, MD ",
                               Val = "24029",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, MD ",
                               Val = "24031",
                           },
                           new GeoFips
                           {
                               Description = "4 Prince George's, MD ",
                               Val = "24033",
                           },
                           new GeoFips
                           {
                               Description = "4 Queen Anne's, MD ",
                               Val = "24035",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Mary's, MD ",
                               Val = "24037",
                           },
                           new GeoFips
                           {
                               Description = "4 Somerset, MD ",
                               Val = "24039",
                           },
                           new GeoFips
                           {
                               Description = "4 Talbot, MD ",
                               Val = "24041",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, MD ",
                               Val = "24043",
                           },
                           new GeoFips
                           {
                               Description = "4 Wicomico, MD ",
                               Val = "24045",
                           },
                           new GeoFips
                           {
                               Description = "4 Worcester, MD ",
                               Val = "24047",
                           },
                           new GeoFips
                           {
                               Description = "5 Goldsboro, NC (Metropolitan Statistical Area)",
                               Val = "24140",
                           },
                           new GeoFips
                           {
                               Description = "5 Grand Forks, ND-MN (Metropolitan Statistical Area)",
                               Val = "24220",
                           },
                           new GeoFips
                           {
                               Description = "5 Grand Island, NE (Metropolitan Statistical Area)",
                               Val = "24260",
                           },
                           new GeoFips
                           {
                               Description = "5 Grand Junction, CO (Metropolitan Statistical Area)",
                               Val = "24300",
                           },
                           new GeoFips
                           {
                               Description = "5 Grand Rapids-Wyoming, MI (Metropolitan Statistical Area)",
                               Val = "24340",
                           },
                           new GeoFips
                           {
                               Description = "5 Grants Pass, OR (Metropolitan Statistical Area)",
                               Val = "24420",
                           },
                           new GeoFips
                           {
                               Description = "5 Great Falls, MT (Metropolitan Statistical Area)",
                               Val = "24500",
                           },
                           new GeoFips
                           {
                               Description = "4 Baltimore (Independent City), MD ",
                               Val = "24510",
                           },
                           new GeoFips
                           {
                               Description = "5 Greeley, CO (Metropolitan Statistical Area)",
                               Val = "24540",
                           },
                           new GeoFips
                           {
                               Description = "5 Green Bay, WI (Metropolitan Statistical Area)",
                               Val = "24580",
                           },
                           new GeoFips
                           {
                               Description = "5 Greensboro-High Point, NC (Metropolitan Statistical Area)",
                               Val = "24660",
                           },
                           new GeoFips
                           {
                               Description = "5 Greenville, NC (Metropolitan Statistical Area)",
                               Val = "24780",
                           },
                           new GeoFips
                           {
                               Description = "5 Greenville-Anderson-Mauldin, SC (Metropolitan Statistical Area)",
                               Val = "24860",
                           },
                           new GeoFips
                           {
                               Description = "3 Massachusetts",
                               Val = "25000",
                           },
                           new GeoFips
                           {
                               Description = "4 Barnstable, MA ",
                               Val = "25001",
                           },
                           new GeoFips
                           {
                               Description = "4 Berkshire, MA ",
                               Val = "25003",
                           },
                           new GeoFips
                           {
                               Description = "4 Bristol, MA ",
                               Val = "25005",
                           },
                           new GeoFips
                           {
                               Description = "4 Dukes, MA ",
                               Val = "25007",
                           },
                           new GeoFips
                           {
                               Description = "4 Essex, MA ",
                               Val = "25009",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, MA ",
                               Val = "25011",
                           },
                           new GeoFips
                           {
                               Description = "4 Hampden, MA ",
                               Val = "25013",
                           },
                           new GeoFips
                           {
                               Description = "4 Hampshire, MA ",
                               Val = "25015",
                           },
                           new GeoFips
                           {
                               Description = "4 Middlesex, MA ",
                               Val = "25017",
                           },
                           new GeoFips
                           {
                               Description = "4 Nantucket, MA ",
                               Val = "25019",
                           },
                           new GeoFips
                           {
                               Description = "4 Norfolk, MA ",
                               Val = "25021",
                           },
                           new GeoFips
                           {
                               Description = "4 Plymouth, MA ",
                               Val = "25023",
                           },
                           new GeoFips
                           {
                               Description = "4 Suffolk, MA ",
                               Val = "25025",
                           },
                           new GeoFips
                           {
                               Description = "4 Worcester, MA ",
                               Val = "25027",
                           },
                           new GeoFips
                           {
                               Description = "5 Gulfport-Biloxi-Pascagoula, MS (Metropolitan Statistical Area)",
                               Val = "25060",
                           },
                           new GeoFips
                           {
                               Description = "5 Hagerstown-Martinsburg, MD-WV (Metropolitan Statistical Area)",
                               Val = "25180",
                           },
                           new GeoFips
                           {
                               Description = "5 Hammond, LA (Metropolitan Statistical Area)",
                               Val = "25220",
                           },
                           new GeoFips
                           {
                               Description = "5 Hanford-Corcoran, CA (Metropolitan Statistical Area)",
                               Val = "25260",
                           },
                           new GeoFips
                           {
                               Description = "5 Harrisburg-Carlisle, PA (Metropolitan Statistical Area)",
                               Val = "25420",
                           },
                           new GeoFips
                           {
                               Description = "5 Harrisonburg, VA (Metropolitan Statistical Area)",
                               Val = "25500",
                           },
                           new GeoFips
                           {
                               Description = "5 Hartford-West Hartford-East Hartford, CT (Metropolitan Statistical Area)",
                               Val = "25540",
                           },
                           new GeoFips
                           {
                               Description = "5 Hattiesburg, MS (Metropolitan Statistical Area)",
                               Val = "25620",
                           },
                           new GeoFips
                           {
                               Description = "5 Hickory-Lenoir-Morganton, NC (Metropolitan Statistical Area)",
                               Val = "25860",
                           },
                           new GeoFips
                           {
                               Description = "5 Hilton Head Island-Bluffton-Beaufort, SC (Metropolitan Statistical Area)",
                               Val = "25940",
                           },
                           new GeoFips
                           {
                               Description = "5 Hinesville, GA (Metropolitan Statistical Area)",
                               Val = "25980",
                           },
                           new GeoFips
                           {
                               Description = "3 Michigan",
                               Val = "26000",
                           },
                           new GeoFips
                           {
                               Description = "4 Alcona, MI ",
                               Val = "26001",
                           },
                           new GeoFips
                           {
                               Description = "4 Alger, MI ",
                               Val = "26003",
                           },
                           new GeoFips
                           {
                               Description = "4 Allegan, MI ",
                               Val = "26005",
                           },
                           new GeoFips
                           {
                               Description = "4 Alpena, MI ",
                               Val = "26007",
                           },
                           new GeoFips
                           {
                               Description = "4 Antrim, MI ",
                               Val = "26009",
                           },
                           new GeoFips
                           {
                               Description = "4 Arenac, MI ",
                               Val = "26011",
                           },
                           new GeoFips
                           {
                               Description = "4 Baraga, MI ",
                               Val = "26013",
                           },
                           new GeoFips
                           {
                               Description = "4 Barry, MI ",
                               Val = "26015",
                           },
                           new GeoFips
                           {
                               Description = "4 Bay, MI ",
                               Val = "26017",
                           },
                           new GeoFips
                           {
                               Description = "4 Benzie, MI ",
                               Val = "26019",
                           },
                           new GeoFips
                           {
                               Description = "4 Berrien, MI ",
                               Val = "26021",
                           },
                           new GeoFips
                           {
                               Description = "4 Branch, MI ",
                               Val = "26023",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, MI ",
                               Val = "26025",
                           },
                           new GeoFips
                           {
                               Description = "4 Cass, MI ",
                               Val = "26027",
                           },
                           new GeoFips
                           {
                               Description = "4 Charlevoix, MI ",
                               Val = "26029",
                           },
                           new GeoFips
                           {
                               Description = "4 Cheboygan, MI ",
                               Val = "26031",
                           },
                           new GeoFips
                           {
                               Description = "4 Chippewa, MI ",
                               Val = "26033",
                           },
                           new GeoFips
                           {
                               Description = "4 Clare, MI ",
                               Val = "26035",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinton, MI ",
                               Val = "26037",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, MI ",
                               Val = "26039",
                           },
                           new GeoFips
                           {
                               Description = "4 Delta, MI ",
                               Val = "26041",
                           },
                           new GeoFips
                           {
                               Description = "4 Dickinson, MI ",
                               Val = "26043",
                           },
                           new GeoFips
                           {
                               Description = "4 Eaton, MI ",
                               Val = "26045",
                           },
                           new GeoFips
                           {
                               Description = "4 Emmet, MI ",
                               Val = "26047",
                           },
                           new GeoFips
                           {
                               Description = "4 Genesee, MI ",
                               Val = "26049",
                           },
                           new GeoFips
                           {
                               Description = "4 Gladwin, MI ",
                               Val = "26051",
                           },
                           new GeoFips
                           {
                               Description = "4 Gogebic, MI ",
                               Val = "26053",
                           },
                           new GeoFips
                           {
                               Description = "4 Grand Traverse, MI ",
                               Val = "26055",
                           },
                           new GeoFips
                           {
                               Description = "4 Gratiot, MI ",
                               Val = "26057",
                           },
                           new GeoFips
                           {
                               Description = "4 Hillsdale, MI ",
                               Val = "26059",
                           },
                           new GeoFips
                           {
                               Description = "4 Houghton, MI ",
                               Val = "26061",
                           },
                           new GeoFips
                           {
                               Description = "4 Huron, MI ",
                               Val = "26063",
                           },
                           new GeoFips
                           {
                               Description = "4 Ingham, MI ",
                               Val = "26065",
                           },
                           new GeoFips
                           {
                               Description = "4 Ionia, MI ",
                               Val = "26067",
                           },
                           new GeoFips
                           {
                               Description = "4 Iosco, MI ",
                               Val = "26069",
                           },
                           new GeoFips
                           {
                               Description = "4 Iron, MI ",
                               Val = "26071",
                           },
                           new GeoFips
                           {
                               Description = "4 Isabella, MI ",
                               Val = "26073",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, MI ",
                               Val = "26075",
                           },
                           new GeoFips
                           {
                               Description = "4 Kalamazoo, MI ",
                               Val = "26077",
                           },
                           new GeoFips
                           {
                               Description = "4 Kalkaska, MI ",
                               Val = "26079",
                           },
                           new GeoFips
                           {
                               Description = "4 Kent, MI ",
                               Val = "26081",
                           },
                           new GeoFips
                           {
                               Description = "4 Keweenaw, MI ",
                               Val = "26083",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, MI ",
                               Val = "26085",
                           },
                           new GeoFips
                           {
                               Description = "4 Lapeer, MI ",
                               Val = "26087",
                           },
                           new GeoFips
                           {
                               Description = "4 Leelanau, MI ",
                               Val = "26089",
                           },
                           new GeoFips
                           {
                               Description = "4 Lenawee, MI ",
                               Val = "26091",
                           },
                           new GeoFips
                           {
                               Description = "4 Livingston, MI ",
                               Val = "26093",
                           },
                           new GeoFips
                           {
                               Description = "4 Luce, MI ",
                               Val = "26095",
                           },
                           new GeoFips
                           {
                               Description = "4 Mackinac, MI ",
                               Val = "26097",
                           },
                           new GeoFips
                           {
                               Description = "4 Macomb, MI ",
                               Val = "26099",
                           },
                           new GeoFips
                           {
                               Description = "4 Manistee, MI ",
                               Val = "26101",
                           },
                           new GeoFips
                           {
                               Description = "4 Marquette, MI ",
                               Val = "26103",
                           },
                           new GeoFips
                           {
                               Description = "4 Mason, MI ",
                               Val = "26105",
                           },
                           new GeoFips
                           {
                               Description = "4 Mecosta, MI ",
                               Val = "26107",
                           },
                           new GeoFips
                           {
                               Description = "4 Menominee, MI ",
                               Val = "26109",
                           },
                           new GeoFips
                           {
                               Description = "4 Midland, MI ",
                               Val = "26111",
                           },
                           new GeoFips
                           {
                               Description = "4 Missaukee, MI ",
                               Val = "26113",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, MI ",
                               Val = "26115",
                           },
                           new GeoFips
                           {
                               Description = "4 Montcalm, MI ",
                               Val = "26117",
                           },
                           new GeoFips
                           {
                               Description = "4 Montmorency, MI ",
                               Val = "26119",
                           },
                           new GeoFips
                           {
                               Description = "4 Muskegon, MI ",
                               Val = "26121",
                           },
                           new GeoFips
                           {
                               Description = "4 Newaygo, MI ",
                               Val = "26123",
                           },
                           new GeoFips
                           {
                               Description = "4 Oakland, MI ",
                               Val = "26125",
                           },
                           new GeoFips
                           {
                               Description = "4 Oceana, MI ",
                               Val = "26127",
                           },
                           new GeoFips
                           {
                               Description = "4 Ogemaw, MI ",
                               Val = "26129",
                           },
                           new GeoFips
                           {
                               Description = "4 Ontonagon, MI ",
                               Val = "26131",
                           },
                           new GeoFips
                           {
                               Description = "4 Osceola, MI ",
                               Val = "26133",
                           },
                           new GeoFips
                           {
                               Description = "4 Oscoda, MI ",
                               Val = "26135",
                           },
                           new GeoFips
                           {
                               Description = "4 Otsego, MI ",
                               Val = "26137",
                           },
                           new GeoFips
                           {
                               Description = "4 Ottawa, MI ",
                               Val = "26139",
                           },
                           new GeoFips
                           {
                               Description = "5 Homosassa Springs, FL (Metropolitan Statistical Area)",
                               Val = "26140",
                           },
                           new GeoFips
                           {
                               Description = "4 Presque Isle, MI ",
                               Val = "26141",
                           },
                           new GeoFips
                           {
                               Description = "4 Roscommon, MI ",
                               Val = "26143",
                           },
                           new GeoFips
                           {
                               Description = "4 Saginaw, MI ",
                               Val = "26145",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Clair, MI ",
                               Val = "26147",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Joseph, MI ",
                               Val = "26149",
                           },
                           new GeoFips
                           {
                               Description = "4 Sanilac, MI ",
                               Val = "26151",
                           },
                           new GeoFips
                           {
                               Description = "4 Schoolcraft, MI ",
                               Val = "26153",
                           },
                           new GeoFips
                           {
                               Description = "4 Shiawassee, MI ",
                               Val = "26155",
                           },
                           new GeoFips
                           {
                               Description = "4 Tuscola, MI ",
                               Val = "26157",
                           },
                           new GeoFips
                           {
                               Description = "4 Van Buren, MI ",
                               Val = "26159",
                           },
                           new GeoFips
                           {
                               Description = "4 Washtenaw, MI ",
                               Val = "26161",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, MI ",
                               Val = "26163",
                           },
                           new GeoFips
                           {
                               Description = "4 Wexford, MI ",
                               Val = "26165",
                           },
                           new GeoFips
                           {
                               Description = "5 Hot Springs, AR (Metropolitan Statistical Area)",
                               Val = "26300",
                           },
                           new GeoFips
                           {
                               Description = "5 Houma-Thibodaux, LA (Metropolitan Statistical Area)",
                               Val = "26380",
                           },
                           new GeoFips
                           {
                               Description = "5 Houston-The Woodlands-Sugar Land, TX (Metropolitan Statistical Area)",
                               Val = "26420",
                           },
                           new GeoFips
                           {
                               Description = "5 Huntington-Ashland, WV-KY-OH (Metropolitan Statistical Area)",
                               Val = "26580",
                           },
                           new GeoFips
                           {
                               Description = "5 Huntsville, AL (Metropolitan Statistical Area)",
                               Val = "26620",
                           },
                           new GeoFips
                           {
                               Description = "5 Idaho Falls, ID (Metropolitan Statistical Area)",
                               Val = "26820",
                           },
                           new GeoFips
                           {
                               Description = "5 Indianapolis-Carmel-Anderson, IN (Metropolitan Statistical Area)",
                               Val = "26900",
                           },
                           new GeoFips
                           {
                               Description = "5 Iowa City, IA (Metropolitan Statistical Area)",
                               Val = "26980",
                           },
                           new GeoFips
                           {
                               Description = "3 Minnesota",
                               Val = "27000",
                           },
                           new GeoFips
                           {
                               Description = "4 Aitkin, MN ",
                               Val = "27001",
                           },
                           new GeoFips
                           {
                               Description = "4 Anoka, MN ",
                               Val = "27003",
                           },
                           new GeoFips
                           {
                               Description = "4 Becker, MN ",
                               Val = "27005",
                           },
                           new GeoFips
                           {
                               Description = "4 Beltrami, MN ",
                               Val = "27007",
                           },
                           new GeoFips
                           {
                               Description = "4 Benton, MN ",
                               Val = "27009",
                           },
                           new GeoFips
                           {
                               Description = "4 Big Stone, MN ",
                               Val = "27011",
                           },
                           new GeoFips
                           {
                               Description = "4 Blue Earth, MN ",
                               Val = "27013",
                           },
                           new GeoFips
                           {
                               Description = "4 Brown, MN ",
                               Val = "27015",
                           },
                           new GeoFips
                           {
                               Description = "4 Carlton, MN ",
                               Val = "27017",
                           },
                           new GeoFips
                           {
                               Description = "4 Carver, MN ",
                               Val = "27019",
                           },
                           new GeoFips
                           {
                               Description = "4 Cass, MN ",
                               Val = "27021",
                           },
                           new GeoFips
                           {
                               Description = "4 Chippewa, MN ",
                               Val = "27023",
                           },
                           new GeoFips
                           {
                               Description = "4 Chisago, MN ",
                               Val = "27025",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, MN ",
                               Val = "27027",
                           },
                           new GeoFips
                           {
                               Description = "4 Clearwater, MN ",
                               Val = "27029",
                           },
                           new GeoFips
                           {
                               Description = "4 Cook, MN ",
                               Val = "27031",
                           },
                           new GeoFips
                           {
                               Description = "4 Cottonwood, MN ",
                               Val = "27033",
                           },
                           new GeoFips
                           {
                               Description = "4 Crow Wing, MN ",
                               Val = "27035",
                           },
                           new GeoFips
                           {
                               Description = "4 Dakota, MN ",
                               Val = "27037",
                           },
                           new GeoFips
                           {
                               Description = "4 Dodge, MN ",
                               Val = "27039",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, MN ",
                               Val = "27041",
                           },
                           new GeoFips
                           {
                               Description = "4 Faribault, MN ",
                               Val = "27043",
                           },
                           new GeoFips
                           {
                               Description = "4 Fillmore, MN ",
                               Val = "27045",
                           },
                           new GeoFips
                           {
                               Description = "4 Freeborn, MN ",
                               Val = "27047",
                           },
                           new GeoFips
                           {
                               Description = "4 Goodhue, MN ",
                               Val = "27049",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, MN ",
                               Val = "27051",
                           },
                           new GeoFips
                           {
                               Description = "4 Hennepin, MN ",
                               Val = "27053",
                           },
                           new GeoFips
                           {
                               Description = "4 Houston, MN ",
                               Val = "27055",
                           },
                           new GeoFips
                           {
                               Description = "4 Hubbard, MN ",
                               Val = "27057",
                           },
                           new GeoFips
                           {
                               Description = "4 Isanti, MN ",
                               Val = "27059",
                           },
                           new GeoFips
                           {
                               Description = "5 Ithaca, NY (Metropolitan Statistical Area)",
                               Val = "27060",
                           },
                           new GeoFips
                           {
                               Description = "4 Itasca, MN ",
                               Val = "27061",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, MN ",
                               Val = "27063",
                           },
                           new GeoFips
                           {
                               Description = "4 Kanabec, MN ",
                               Val = "27065",
                           },
                           new GeoFips
                           {
                               Description = "4 Kandiyohi, MN ",
                               Val = "27067",
                           },
                           new GeoFips
                           {
                               Description = "4 Kittson, MN ",
                               Val = "27069",
                           },
                           new GeoFips
                           {
                               Description = "4 Koochiching, MN ",
                               Val = "27071",
                           },
                           new GeoFips
                           {
                               Description = "4 Lac qui Parle, MN ",
                               Val = "27073",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, MN ",
                               Val = "27075",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake of the Woods, MN ",
                               Val = "27077",
                           },
                           new GeoFips
                           {
                               Description = "4 Le Sueur, MN ",
                               Val = "27079",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, MN ",
                               Val = "27081",
                           },
                           new GeoFips
                           {
                               Description = "4 Lyon, MN ",
                               Val = "27083",
                           },
                           new GeoFips
                           {
                               Description = "4 McLeod, MN ",
                               Val = "27085",
                           },
                           new GeoFips
                           {
                               Description = "4 Mahnomen, MN ",
                               Val = "27087",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, MN ",
                               Val = "27089",
                           },
                           new GeoFips
                           {
                               Description = "4 Martin, MN ",
                               Val = "27091",
                           },
                           new GeoFips
                           {
                               Description = "4 Meeker, MN ",
                               Val = "27093",
                           },
                           new GeoFips
                           {
                               Description = "4 Mille Lacs, MN ",
                               Val = "27095",
                           },
                           new GeoFips
                           {
                               Description = "4 Morrison, MN ",
                               Val = "27097",
                           },
                           new GeoFips
                           {
                               Description = "4 Mower, MN ",
                               Val = "27099",
                           },
                           new GeoFips
                           {
                               Description = "5 Jackson, MI (Metropolitan Statistical Area)",
                               Val = "27100",
                           },
                           new GeoFips
                           {
                               Description = "4 Murray, MN ",
                               Val = "27101",
                           },
                           new GeoFips
                           {
                               Description = "4 Nicollet, MN ",
                               Val = "27103",
                           },
                           new GeoFips
                           {
                               Description = "4 Nobles, MN ",
                               Val = "27105",
                           },
                           new GeoFips
                           {
                               Description = "4 Norman, MN ",
                               Val = "27107",
                           },
                           new GeoFips
                           {
                               Description = "4 Olmsted, MN ",
                               Val = "27109",
                           },
                           new GeoFips
                           {
                               Description = "4 Otter Tail, MN ",
                               Val = "27111",
                           },
                           new GeoFips
                           {
                               Description = "4 Pennington, MN ",
                               Val = "27113",
                           },
                           new GeoFips
                           {
                               Description = "4 Pine, MN ",
                               Val = "27115",
                           },
                           new GeoFips
                           {
                               Description = "4 Pipestone, MN ",
                               Val = "27117",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, MN ",
                               Val = "27119",
                           },
                           new GeoFips
                           {
                               Description = "4 Pope, MN ",
                               Val = "27121",
                           },
                           new GeoFips
                           {
                               Description = "4 Ramsey, MN ",
                               Val = "27123",
                           },
                           new GeoFips
                           {
                               Description = "4 Red Lake, MN ",
                               Val = "27125",
                           },
                           new GeoFips
                           {
                               Description = "4 Redwood, MN ",
                               Val = "27127",
                           },
                           new GeoFips
                           {
                               Description = "4 Renville, MN ",
                               Val = "27129",
                           },
                           new GeoFips
                           {
                               Description = "4 Rice, MN ",
                               Val = "27131",
                           },
                           new GeoFips
                           {
                               Description = "4 Rock, MN ",
                               Val = "27133",
                           },
                           new GeoFips
                           {
                               Description = "4 Roseau, MN ",
                               Val = "27135",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Louis, MN ",
                               Val = "27137",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, MN ",
                               Val = "27139",
                           },
                           new GeoFips
                           {
                               Description = "5 Jackson, MS (Metropolitan Statistical Area)",
                               Val = "27140",
                           },
                           new GeoFips
                           {
                               Description = "4 Sherburne, MN ",
                               Val = "27141",
                           },
                           new GeoFips
                           {
                               Description = "4 Sibley, MN ",
                               Val = "27143",
                           },
                           new GeoFips
                           {
                               Description = "4 Stearns, MN ",
                               Val = "27145",
                           },
                           new GeoFips
                           {
                               Description = "4 Steele, MN ",
                               Val = "27147",
                           },
                           new GeoFips
                           {
                               Description = "4 Stevens, MN ",
                               Val = "27149",
                           },
                           new GeoFips
                           {
                               Description = "4 Swift, MN ",
                               Val = "27151",
                           },
                           new GeoFips
                           {
                               Description = "4 Todd, MN ",
                               Val = "27153",
                           },
                           new GeoFips
                           {
                               Description = "4 Traverse, MN ",
                               Val = "27155",
                           },
                           new GeoFips
                           {
                               Description = "4 Wabasha, MN ",
                               Val = "27157",
                           },
                           new GeoFips
                           {
                               Description = "4 Wadena, MN ",
                               Val = "27159",
                           },
                           new GeoFips
                           {
                               Description = "4 Waseca, MN ",
                               Val = "27161",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, MN ",
                               Val = "27163",
                           },
                           new GeoFips
                           {
                               Description = "4 Watonwan, MN ",
                               Val = "27165",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilkin, MN ",
                               Val = "27167",
                           },
                           new GeoFips
                           {
                               Description = "4 Winona, MN ",
                               Val = "27169",
                           },
                           new GeoFips
                           {
                               Description = "4 Wright, MN ",
                               Val = "27171",
                           },
                           new GeoFips
                           {
                               Description = "4 Yellow Medicine, MN ",
                               Val = "27173",
                           },
                           new GeoFips
                           {
                               Description = "5 Jackson, TN (Metropolitan Statistical Area)",
                               Val = "27180",
                           },
                           new GeoFips
                           {
                               Description = "5 Jacksonville, FL (Metropolitan Statistical Area)",
                               Val = "27260",
                           },
                           new GeoFips
                           {
                               Description = "5 Jacksonville, NC (Metropolitan Statistical Area)",
                               Val = "27340",
                           },
                           new GeoFips
                           {
                               Description = "5 Janesville-Beloit, WI (Metropolitan Statistical Area)",
                               Val = "27500",
                           },
                           new GeoFips
                           {
                               Description = "5 Jefferson City, MO (Metropolitan Statistical Area)",
                               Val = "27620",
                           },
                           new GeoFips
                           {
                               Description = "5 Johnson City, TN (Metropolitan Statistical Area)",
                               Val = "27740",
                           },
                           new GeoFips
                           {
                               Description = "5 Johnstown, PA (Metropolitan Statistical Area)",
                               Val = "27780",
                           },
                           new GeoFips
                           {
                               Description = "5 Jonesboro, AR (Metropolitan Statistical Area)",
                               Val = "27860",
                           },
                           new GeoFips
                           {
                               Description = "5 Joplin, MO (Metropolitan Statistical Area)",
                               Val = "27900",
                           },
                           new GeoFips
                           {
                               Description = "5 Kahului-Wailuku-Lahaina, HI (Metropolitan Statistical Area)",
                               Val = "27980",
                           },
                           new GeoFips
                           {
                               Description = "3 Mississippi",
                               Val = "28000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, MS ",
                               Val = "28001",
                           },
                           new GeoFips
                           {
                               Description = "4 Alcorn, MS ",
                               Val = "28003",
                           },
                           new GeoFips
                           {
                               Description = "4 Amite, MS ",
                               Val = "28005",
                           },
                           new GeoFips
                           {
                               Description = "4 Attala, MS ",
                               Val = "28007",
                           },
                           new GeoFips
                           {
                               Description = "4 Benton, MS ",
                               Val = "28009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bolivar, MS ",
                               Val = "28011",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, MS ",
                               Val = "28013",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, MS ",
                               Val = "28015",
                           },
                           new GeoFips
                           {
                               Description = "4 Chickasaw, MS ",
                               Val = "28017",
                           },
                           new GeoFips
                           {
                               Description = "4 Choctaw, MS ",
                               Val = "28019",
                           },
                           new GeoFips
                           {
                               Description = "5 Kalamazoo-Portage, MI (Metropolitan Statistical Area)",
                               Val = "28020",
                           },
                           new GeoFips
                           {
                               Description = "4 Claiborne, MS ",
                               Val = "28021",
                           },
                           new GeoFips
                           {
                               Description = "4 Clarke, MS ",
                               Val = "28023",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, MS ",
                               Val = "28025",
                           },
                           new GeoFips
                           {
                               Description = "4 Coahoma, MS ",
                               Val = "28027",
                           },
                           new GeoFips
                           {
                               Description = "4 Copiah, MS ",
                               Val = "28029",
                           },
                           new GeoFips
                           {
                               Description = "4 Covington, MS ",
                               Val = "28031",
                           },
                           new GeoFips
                           {
                               Description = "4 DeSoto, MS ",
                               Val = "28033",
                           },
                           new GeoFips
                           {
                               Description = "4 Forrest, MS ",
                               Val = "28035",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, MS ",
                               Val = "28037",
                           },
                           new GeoFips
                           {
                               Description = "4 George, MS ",
                               Val = "28039",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, MS ",
                               Val = "28041",
                           },
                           new GeoFips
                           {
                               Description = "4 Grenada, MS ",
                               Val = "28043",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, MS ",
                               Val = "28045",
                           },
                           new GeoFips
                           {
                               Description = "4 Harrison, MS ",
                               Val = "28047",
                           },
                           new GeoFips
                           {
                               Description = "4 Hinds, MS ",
                               Val = "28049",
                           },
                           new GeoFips
                           {
                               Description = "4 Holmes, MS ",
                               Val = "28051",
                           },
                           new GeoFips
                           {
                               Description = "4 Humphreys, MS ",
                               Val = "28053",
                           },
                           new GeoFips
                           {
                               Description = "4 Issaquena, MS ",
                               Val = "28055",
                           },
                           new GeoFips
                           {
                               Description = "4 Itawamba, MS ",
                               Val = "28057",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, MS ",
                               Val = "28059",
                           },
                           new GeoFips
                           {
                               Description = "4 Jasper, MS ",
                               Val = "28061",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, MS ",
                               Val = "28063",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson Davis, MS ",
                               Val = "28065",
                           },
                           new GeoFips
                           {
                               Description = "4 Jones, MS ",
                               Val = "28067",
                           },
                           new GeoFips
                           {
                               Description = "4 Kemper, MS ",
                               Val = "28069",
                           },
                           new GeoFips
                           {
                               Description = "4 Lafayette, MS ",
                               Val = "28071",
                           },
                           new GeoFips
                           {
                               Description = "4 Lamar, MS ",
                               Val = "28073",
                           },
                           new GeoFips
                           {
                               Description = "4 Lauderdale, MS ",
                               Val = "28075",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, MS ",
                               Val = "28077",
                           },
                           new GeoFips
                           {
                               Description = "4 Leake, MS ",
                               Val = "28079",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, MS ",
                               Val = "28081",
                           },
                           new GeoFips
                           {
                               Description = "4 Leflore, MS ",
                               Val = "28083",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, MS ",
                               Val = "28085",
                           },
                           new GeoFips
                           {
                               Description = "4 Lowndes, MS ",
                               Val = "28087",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, MS ",
                               Val = "28089",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, MS ",
                               Val = "28091",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, MS ",
                               Val = "28093",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, MS ",
                               Val = "28095",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, MS ",
                               Val = "28097",
                           },
                           new GeoFips
                           {
                               Description = "4 Neshoba, MS ",
                               Val = "28099",
                           },
                           new GeoFips
                           {
                               Description = "5 Kankakee, IL (Metropolitan Statistical Area)",
                               Val = "28100",
                           },
                           new GeoFips
                           {
                               Description = "4 Newton, MS ",
                               Val = "28101",
                           },
                           new GeoFips
                           {
                               Description = "4 Noxubee, MS ",
                               Val = "28103",
                           },
                           new GeoFips
                           {
                               Description = "4 Oktibbeha, MS ",
                               Val = "28105",
                           },
                           new GeoFips
                           {
                               Description = "4 Panola, MS ",
                               Val = "28107",
                           },
                           new GeoFips
                           {
                               Description = "4 Pearl River, MS ",
                               Val = "28109",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, MS ",
                               Val = "28111",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, MS ",
                               Val = "28113",
                           },
                           new GeoFips
                           {
                               Description = "4 Pontotoc, MS ",
                               Val = "28115",
                           },
                           new GeoFips
                           {
                               Description = "4 Prentiss, MS ",
                               Val = "28117",
                           },
                           new GeoFips
                           {
                               Description = "4 Quitman, MS ",
                               Val = "28119",
                           },
                           new GeoFips
                           {
                               Description = "4 Rankin, MS ",
                               Val = "28121",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, MS ",
                               Val = "28123",
                           },
                           new GeoFips
                           {
                               Description = "4 Sharkey, MS ",
                               Val = "28125",
                           },
                           new GeoFips
                           {
                               Description = "4 Simpson, MS ",
                               Val = "28127",
                           },
                           new GeoFips
                           {
                               Description = "4 Smith, MS ",
                               Val = "28129",
                           },
                           new GeoFips
                           {
                               Description = "4 Stone, MS ",
                               Val = "28131",
                           },
                           new GeoFips
                           {
                               Description = "4 Sunflower, MS ",
                               Val = "28133",
                           },
                           new GeoFips
                           {
                               Description = "4 Tallahatchie, MS ",
                               Val = "28135",
                           },
                           new GeoFips
                           {
                               Description = "4 Tate, MS ",
                               Val = "28137",
                           },
                           new GeoFips
                           {
                               Description = "4 Tippah, MS ",
                               Val = "28139",
                           },
                           new GeoFips
                           {
                               Description = "5 Kansas City, MO-KS (Metropolitan Statistical Area)",
                               Val = "28140",
                           },
                           new GeoFips
                           {
                               Description = "4 Tishomingo, MS ",
                               Val = "28141",
                           },
                           new GeoFips
                           {
                               Description = "4 Tunica, MS ",
                               Val = "28143",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, MS ",
                               Val = "28145",
                           },
                           new GeoFips
                           {
                               Description = "4 Walthall, MS ",
                               Val = "28147",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, MS ",
                               Val = "28149",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, MS ",
                               Val = "28151",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, MS ",
                               Val = "28153",
                           },
                           new GeoFips
                           {
                               Description = "4 Webster, MS ",
                               Val = "28155",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilkinson, MS ",
                               Val = "28157",
                           },
                           new GeoFips
                           {
                               Description = "4 Winston, MS ",
                               Val = "28159",
                           },
                           new GeoFips
                           {
                               Description = "4 Yalobusha, MS ",
                               Val = "28161",
                           },
                           new GeoFips
                           {
                               Description = "4 Yazoo, MS ",
                               Val = "28163",
                           },
                           new GeoFips
                           {
                               Description = "5 Kennewick-Richland, WA (Metropolitan Statistical Area)",
                               Val = "28420",
                           },
                           new GeoFips
                           {
                               Description = "5 Killeen-Temple, TX (Metropolitan Statistical Area)",
                               Val = "28660",
                           },
                           new GeoFips
                           {
                               Description = "5 Kingsport-Bristol-Bristol, TN-VA (Metropolitan Statistical Area)",
                               Val = "28700",
                           },
                           new GeoFips
                           {
                               Description = "5 Kingston, NY (Metropolitan Statistical Area)",
                               Val = "28740",
                           },
                           new GeoFips
                           {
                               Description = "5 Knoxville, TN (Metropolitan Statistical Area)",
                               Val = "28940",
                           },
                           new GeoFips
                           {
                               Description = "3 Missouri",
                               Val = "29000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adair, MO ",
                               Val = "29001",
                           },
                           new GeoFips
                           {
                               Description = "4 Andrew, MO ",
                               Val = "29003",
                           },
                           new GeoFips
                           {
                               Description = "4 Atchison, MO ",
                               Val = "29005",
                           },
                           new GeoFips
                           {
                               Description = "4 Audrain, MO ",
                               Val = "29007",
                           },
                           new GeoFips
                           {
                               Description = "4 Barry, MO ",
                               Val = "29009",
                           },
                           new GeoFips
                           {
                               Description = "4 Barton, MO ",
                               Val = "29011",
                           },
                           new GeoFips
                           {
                               Description = "4 Bates, MO ",
                               Val = "29013",
                           },
                           new GeoFips
                           {
                               Description = "4 Benton, MO ",
                               Val = "29015",
                           },
                           new GeoFips
                           {
                               Description = "4 Bollinger, MO ",
                               Val = "29017",
                           },
                           new GeoFips
                           {
                               Description = "4 Boone, MO ",
                               Val = "29019",
                           },
                           new GeoFips
                           {
                               Description = "5 Kokomo, IN (Metropolitan Statistical Area)",
                               Val = "29020",
                           },
                           new GeoFips
                           {
                               Description = "4 Buchanan, MO ",
                               Val = "29021",
                           },
                           new GeoFips
                           {
                               Description = "4 Butler, MO ",
                               Val = "29023",
                           },
                           new GeoFips
                           {
                               Description = "4 Caldwell, MO ",
                               Val = "29025",
                           },
                           new GeoFips
                           {
                               Description = "4 Callaway, MO ",
                               Val = "29027",
                           },
                           new GeoFips
                           {
                               Description = "4 Camden, MO ",
                               Val = "29029",
                           },
                           new GeoFips
                           {
                               Description = "4 Cape Girardeau, MO ",
                               Val = "29031",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, MO ",
                               Val = "29033",
                           },
                           new GeoFips
                           {
                               Description = "4 Carter, MO ",
                               Val = "29035",
                           },
                           new GeoFips
                           {
                               Description = "4 Cass, MO ",
                               Val = "29037",
                           },
                           new GeoFips
                           {
                               Description = "4 Cedar, MO ",
                               Val = "29039",
                           },
                           new GeoFips
                           {
                               Description = "4 Chariton, MO ",
                               Val = "29041",
                           },
                           new GeoFips
                           {
                               Description = "4 Christian, MO ",
                               Val = "29043",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, MO ",
                               Val = "29045",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, MO ",
                               Val = "29047",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinton, MO ",
                               Val = "29049",
                           },
                           new GeoFips
                           {
                               Description = "4 Cole, MO ",
                               Val = "29051",
                           },
                           new GeoFips
                           {
                               Description = "4 Cooper, MO ",
                               Val = "29053",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, MO ",
                               Val = "29055",
                           },
                           new GeoFips
                           {
                               Description = "4 Dade, MO ",
                               Val = "29057",
                           },
                           new GeoFips
                           {
                               Description = "4 Dallas, MO ",
                               Val = "29059",
                           },
                           new GeoFips
                           {
                               Description = "4 Daviess, MO ",
                               Val = "29061",
                           },
                           new GeoFips
                           {
                               Description = "4 DeKalb, MO ",
                               Val = "29063",
                           },
                           new GeoFips
                           {
                               Description = "4 Dent, MO ",
                               Val = "29065",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, MO ",
                               Val = "29067",
                           },
                           new GeoFips
                           {
                               Description = "4 Dunklin, MO ",
                               Val = "29069",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, MO ",
                               Val = "29071",
                           },
                           new GeoFips
                           {
                               Description = "4 Gasconade, MO ",
                               Val = "29073",
                           },
                           new GeoFips
                           {
                               Description = "4 Gentry, MO ",
                               Val = "29075",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, MO ",
                               Val = "29077",
                           },
                           new GeoFips
                           {
                               Description = "4 Grundy, MO ",
                               Val = "29079",
                           },
                           new GeoFips
                           {
                               Description = "4 Harrison, MO ",
                               Val = "29081",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry, MO ",
                               Val = "29083",
                           },
                           new GeoFips
                           {
                               Description = "4 Hickory, MO ",
                               Val = "29085",
                           },
                           new GeoFips
                           {
                               Description = "4 Holt, MO ",
                               Val = "29087",
                           },
                           new GeoFips
                           {
                               Description = "4 Howard, MO ",
                               Val = "29089",
                           },
                           new GeoFips
                           {
                               Description = "4 Howell, MO ",
                               Val = "29091",
                           },
                           new GeoFips
                           {
                               Description = "4 Iron, MO ",
                               Val = "29093",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, MO ",
                               Val = "29095",
                           },
                           new GeoFips
                           {
                               Description = "4 Jasper, MO ",
                               Val = "29097",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, MO ",
                               Val = "29099",
                           },
                           new GeoFips
                           {
                               Description = "5 La Crosse-Onalaska, WI-MN (Metropolitan Statistical Area)",
                               Val = "29100",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, MO ",
                               Val = "29101",
                           },
                           new GeoFips
                           {
                               Description = "4 Knox, MO ",
                               Val = "29103",
                           },
                           new GeoFips
                           {
                               Description = "4 Laclede, MO ",
                               Val = "29105",
                           },
                           new GeoFips
                           {
                               Description = "4 Lafayette, MO ",
                               Val = "29107",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, MO ",
                               Val = "29109",
                           },
                           new GeoFips
                           {
                               Description = "4 Lewis, MO ",
                               Val = "29111",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, MO ",
                               Val = "29113",
                           },
                           new GeoFips
                           {
                               Description = "4 Linn, MO ",
                               Val = "29115",
                           },
                           new GeoFips
                           {
                               Description = "4 Livingston, MO ",
                               Val = "29117",
                           },
                           new GeoFips
                           {
                               Description = "4 McDonald, MO ",
                               Val = "29119",
                           },
                           new GeoFips
                           {
                               Description = "4 Macon, MO ",
                               Val = "29121",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, MO ",
                               Val = "29123",
                           },
                           new GeoFips
                           {
                               Description = "4 Maries, MO ",
                               Val = "29125",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, MO ",
                               Val = "29127",
                           },
                           new GeoFips
                           {
                               Description = "4 Mercer, MO ",
                               Val = "29129",
                           },
                           new GeoFips
                           {
                               Description = "4 Miller, MO ",
                               Val = "29131",
                           },
                           new GeoFips
                           {
                               Description = "4 Mississippi, MO ",
                               Val = "29133",
                           },
                           new GeoFips
                           {
                               Description = "4 Moniteau, MO ",
                               Val = "29135",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, MO ",
                               Val = "29137",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, MO ",
                               Val = "29139",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, MO ",
                               Val = "29141",
                           },
                           new GeoFips
                           {
                               Description = "4 New Madrid, MO ",
                               Val = "29143",
                           },
                           new GeoFips
                           {
                               Description = "4 Newton, MO ",
                               Val = "29145",
                           },
                           new GeoFips
                           {
                               Description = "4 Nodaway, MO ",
                               Val = "29147",
                           },
                           new GeoFips
                           {
                               Description = "4 Oregon, MO ",
                               Val = "29149",
                           },
                           new GeoFips
                           {
                               Description = "4 Osage, MO ",
                               Val = "29151",
                           },
                           new GeoFips
                           {
                               Description = "4 Ozark, MO ",
                               Val = "29153",
                           },
                           new GeoFips
                           {
                               Description = "4 Pemiscot, MO ",
                               Val = "29155",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, MO ",
                               Val = "29157",
                           },
                           new GeoFips
                           {
                               Description = "4 Pettis, MO ",
                               Val = "29159",
                           },
                           new GeoFips
                           {
                               Description = "4 Phelps, MO ",
                               Val = "29161",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, MO ",
                               Val = "29163",
                           },
                           new GeoFips
                           {
                               Description = "4 Platte, MO ",
                               Val = "29165",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, MO ",
                               Val = "29167",
                           },
                           new GeoFips
                           {
                               Description = "4 Pulaski, MO ",
                               Val = "29169",
                           },
                           new GeoFips
                           {
                               Description = "4 Putnam, MO ",
                               Val = "29171",
                           },
                           new GeoFips
                           {
                               Description = "4 Ralls, MO ",
                               Val = "29173",
                           },
                           new GeoFips
                           {
                               Description = "4 Randolph, MO ",
                               Val = "29175",
                           },
                           new GeoFips
                           {
                               Description = "4 Ray, MO ",
                               Val = "29177",
                           },
                           new GeoFips
                           {
                               Description = "4 Reynolds, MO ",
                               Val = "29179",
                           },
                           new GeoFips
                           {
                               Description = "5 Lafayette, LA (Metropolitan Statistical Area)",
                               Val = "29180",
                           },
                           new GeoFips
                           {
                               Description = "4 Ripley, MO ",
                               Val = "29181",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Charles, MO ",
                               Val = "29183",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Clair, MO ",
                               Val = "29185",
                           },
                           new GeoFips
                           {
                               Description = "4 Ste. Genevieve, MO ",
                               Val = "29186",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Francois, MO ",
                               Val = "29187",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Louis, MO ",
                               Val = "29189",
                           },
                           new GeoFips
                           {
                               Description = "4 Saline, MO ",
                               Val = "29195",
                           },
                           new GeoFips
                           {
                               Description = "4 Schuyler, MO ",
                               Val = "29197",
                           },
                           new GeoFips
                           {
                               Description = "4 Scotland, MO ",
                               Val = "29199",
                           },
                           new GeoFips
                           {
                               Description = "5 Lafayette-West Lafayette, IN (Metropolitan Statistical Area)",
                               Val = "29200",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, MO ",
                               Val = "29201",
                           },
                           new GeoFips
                           {
                               Description = "4 Shannon, MO ",
                               Val = "29203",
                           },
                           new GeoFips
                           {
                               Description = "4 Shelby, MO ",
                               Val = "29205",
                           },
                           new GeoFips
                           {
                               Description = "4 Stoddard, MO ",
                               Val = "29207",
                           },
                           new GeoFips
                           {
                               Description = "4 Stone, MO ",
                               Val = "29209",
                           },
                           new GeoFips
                           {
                               Description = "4 Sullivan, MO ",
                               Val = "29211",
                           },
                           new GeoFips
                           {
                               Description = "4 Taney, MO ",
                               Val = "29213",
                           },
                           new GeoFips
                           {
                               Description = "4 Texas, MO ",
                               Val = "29215",
                           },
                           new GeoFips
                           {
                               Description = "4 Vernon, MO ",
                               Val = "29217",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, MO ",
                               Val = "29219",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, MO ",
                               Val = "29221",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, MO ",
                               Val = "29223",
                           },
                           new GeoFips
                           {
                               Description = "4 Webster, MO ",
                               Val = "29225",
                           },
                           new GeoFips
                           {
                               Description = "4 Worth, MO ",
                               Val = "29227",
                           },
                           new GeoFips
                           {
                               Description = "4 Wright, MO ",
                               Val = "29229",
                           },
                           new GeoFips
                           {
                               Description = "5 Lake Charles, LA (Metropolitan Statistical Area)",
                               Val = "29340",
                           },
                           new GeoFips
                           {
                               Description = "5 Lake Havasu City-Kingman, AZ (Metropolitan Statistical Area)",
                               Val = "29420",
                           },
                           new GeoFips
                           {
                               Description = "5 Lakeland-Winter Haven, FL (Metropolitan Statistical Area)",
                               Val = "29460",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Louis (Independent City), MO ",
                               Val = "29510",
                           },
                           new GeoFips
                           {
                               Description = "5 Lancaster, PA (Metropolitan Statistical Area)",
                               Val = "29540",
                           },
                           new GeoFips
                           {
                               Description = "5 Lansing-East Lansing, MI (Metropolitan Statistical Area)",
                               Val = "29620",
                           },
                           new GeoFips
                           {
                               Description = "5 Laredo, TX (Metropolitan Statistical Area)",
                               Val = "29700",
                           },
                           new GeoFips
                           {
                               Description = "5 Las Cruces, NM (Metropolitan Statistical Area)",
                               Val = "29740",
                           },
                           new GeoFips
                           {
                               Description = "5 Las Vegas-Henderson-Paradise, NV (Metropolitan Statistical Area)",
                               Val = "29820",
                           },
                           new GeoFips
                           {
                               Description = "5 Lawrence, KS (Metropolitan Statistical Area)",
                               Val = "29940",
                           },
                           new GeoFips
                           {
                               Description = "3 Montana",
                               Val = "30000",
                           },
                           new GeoFips
                           {
                               Description = "4 Beaverhead, MT ",
                               Val = "30001",
                           },
                           new GeoFips
                           {
                               Description = "4 Big Horn, MT ",
                               Val = "30003",
                           },
                           new GeoFips
                           {
                               Description = "4 Blaine, MT ",
                               Val = "30005",
                           },
                           new GeoFips
                           {
                               Description = "4 Broadwater, MT ",
                               Val = "30007",
                           },
                           new GeoFips
                           {
                               Description = "4 Carbon, MT ",
                               Val = "30009",
                           },
                           new GeoFips
                           {
                               Description = "4 Carter, MT ",
                               Val = "30011",
                           },
                           new GeoFips
                           {
                               Description = "4 Cascade, MT ",
                               Val = "30013",
                           },
                           new GeoFips
                           {
                               Description = "4 Chouteau, MT ",
                               Val = "30015",
                           },
                           new GeoFips
                           {
                               Description = "4 Custer, MT ",
                               Val = "30017",
                           },
                           new GeoFips
                           {
                               Description = "4 Daniels, MT ",
                               Val = "30019",
                           },
                           new GeoFips
                           {
                               Description = "5 Lawton, OK (Metropolitan Statistical Area)",
                               Val = "30020",
                           },
                           new GeoFips
                           {
                               Description = "4 Dawson, MT ",
                               Val = "30021",
                           },
                           new GeoFips
                           {
                               Description = "4 Deer Lodge, MT ",
                               Val = "30023",
                           },
                           new GeoFips
                           {
                               Description = "4 Fallon, MT ",
                               Val = "30025",
                           },
                           new GeoFips
                           {
                               Description = "4 Fergus, MT ",
                               Val = "30027",
                           },
                           new GeoFips
                           {
                               Description = "4 Flathead, MT ",
                               Val = "30029",
                           },
                           new GeoFips
                           {
                               Description = "4 Gallatin, MT ",
                               Val = "30031",
                           },
                           new GeoFips
                           {
                               Description = "4 Garfield, MT ",
                               Val = "30033",
                           },
                           new GeoFips
                           {
                               Description = "4 Glacier, MT ",
                               Val = "30035",
                           },
                           new GeoFips
                           {
                               Description = "4 Golden Valley, MT ",
                               Val = "30037",
                           },
                           new GeoFips
                           {
                               Description = "4 Granite, MT ",
                               Val = "30039",
                           },
                           new GeoFips
                           {
                               Description = "4 Hill, MT ",
                               Val = "30041",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, MT ",
                               Val = "30043",
                           },
                           new GeoFips
                           {
                               Description = "4 Judith Basin, MT ",
                               Val = "30045",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, MT ",
                               Val = "30047",
                           },
                           new GeoFips
                           {
                               Description = "4 Lewis and Clark, MT ",
                               Val = "30049",
                           },
                           new GeoFips
                           {
                               Description = "4 Liberty, MT ",
                               Val = "30051",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, MT ",
                               Val = "30053",
                           },
                           new GeoFips
                           {
                               Description = "4 McCone, MT ",
                               Val = "30055",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, MT ",
                               Val = "30057",
                           },
                           new GeoFips
                           {
                               Description = "4 Meagher, MT ",
                               Val = "30059",
                           },
                           new GeoFips
                           {
                               Description = "4 Mineral, MT ",
                               Val = "30061",
                           },
                           new GeoFips
                           {
                               Description = "4 Missoula, MT ",
                               Val = "30063",
                           },
                           new GeoFips
                           {
                               Description = "4 Musselshell, MT ",
                               Val = "30065",
                           },
                           new GeoFips
                           {
                               Description = "4 Park, MT ",
                               Val = "30067",
                           },
                           new GeoFips
                           {
                               Description = "4 Petroleum, MT ",
                               Val = "30069",
                           },
                           new GeoFips
                           {
                               Description = "4 Phillips, MT ",
                               Val = "30071",
                           },
                           new GeoFips
                           {
                               Description = "4 Pondera, MT ",
                               Val = "30073",
                           },
                           new GeoFips
                           {
                               Description = "4 Powder River, MT ",
                               Val = "30075",
                           },
                           new GeoFips
                           {
                               Description = "4 Powell, MT ",
                               Val = "30077",
                           },
                           new GeoFips
                           {
                               Description = "4 Prairie, MT ",
                               Val = "30079",
                           },
                           new GeoFips
                           {
                               Description = "4 Ravalli, MT ",
                               Val = "30081",
                           },
                           new GeoFips
                           {
                               Description = "4 Richland, MT ",
                               Val = "30083",
                           },
                           new GeoFips
                           {
                               Description = "4 Roosevelt, MT ",
                               Val = "30085",
                           },
                           new GeoFips
                           {
                               Description = "4 Rosebud, MT ",
                               Val = "30087",
                           },
                           new GeoFips
                           {
                               Description = "4 Sanders, MT ",
                               Val = "30089",
                           },
                           new GeoFips
                           {
                               Description = "4 Sheridan, MT ",
                               Val = "30091",
                           },
                           new GeoFips
                           {
                               Description = "4 Silver Bow, MT ",
                               Val = "30093",
                           },
                           new GeoFips
                           {
                               Description = "4 Stillwater, MT ",
                               Val = "30095",
                           },
                           new GeoFips
                           {
                               Description = "4 Sweet Grass, MT ",
                               Val = "30097",
                           },
                           new GeoFips
                           {
                               Description = "4 Teton, MT ",
                               Val = "30099",
                           },
                           new GeoFips
                           {
                               Description = "4 Toole, MT ",
                               Val = "30101",
                           },
                           new GeoFips
                           {
                               Description = "4 Treasure, MT ",
                               Val = "30103",
                           },
                           new GeoFips
                           {
                               Description = "4 Valley, MT ",
                               Val = "30105",
                           },
                           new GeoFips
                           {
                               Description = "4 Wheatland, MT ",
                               Val = "30107",
                           },
                           new GeoFips
                           {
                               Description = "4 Wibaux, MT ",
                               Val = "30109",
                           },
                           new GeoFips
                           {
                               Description = "4 Yellowstone, MT ",
                               Val = "30111",
                           },
                           new GeoFips
                           {
                               Description = "5 Lebanon, PA (Metropolitan Statistical Area)",
                               Val = "30140",
                           },
                           new GeoFips
                           {
                               Description = "5 Lewiston, ID-WA (Metropolitan Statistical Area)",
                               Val = "30300",
                           },
                           new GeoFips
                           {
                               Description = "5 Lewiston-Auburn, ME (Metropolitan Statistical Area)",
                               Val = "30340",
                           },
                           new GeoFips
                           {
                               Description = "5 Lexington-Fayette, KY (Metropolitan Statistical Area)",
                               Val = "30460",
                           },
                           new GeoFips
                           {
                               Description = "5 Lima, OH (Metropolitan Statistical Area)",
                               Val = "30620",
                           },
                           new GeoFips
                           {
                               Description = "5 Lincoln, NE (Metropolitan Statistical Area)",
                               Val = "30700",
                           },
                           new GeoFips
                           {
                               Description = "5 Little Rock-North Little Rock-Conway, AR (Metropolitan Statistical Area)",
                               Val = "30780",
                           },
                           new GeoFips
                           {
                               Description = "5 Logan, UT-ID (Metropolitan Statistical Area)",
                               Val = "30860",
                           },
                           new GeoFips
                           {
                               Description = "5 Longview, TX (Metropolitan Statistical Area)",
                               Val = "30980",
                           },
                           new GeoFips
                           {
                               Description = "3 Nebraska",
                               Val = "31000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, NE ",
                               Val = "31001",
                           },
                           new GeoFips
                           {
                               Description = "4 Antelope, NE ",
                               Val = "31003",
                           },
                           new GeoFips
                           {
                               Description = "4 Arthur, NE ",
                               Val = "31005",
                           },
                           new GeoFips
                           {
                               Description = "4 Banner, NE ",
                               Val = "31007",
                           },
                           new GeoFips
                           {
                               Description = "4 Blaine, NE ",
                               Val = "31009",
                           },
                           new GeoFips
                           {
                               Description = "4 Boone, NE ",
                               Val = "31011",
                           },
                           new GeoFips
                           {
                               Description = "4 Box Butte, NE ",
                               Val = "31013",
                           },
                           new GeoFips
                           {
                               Description = "4 Boyd, NE ",
                               Val = "31015",
                           },
                           new GeoFips
                           {
                               Description = "4 Brown, NE ",
                               Val = "31017",
                           },
                           new GeoFips
                           {
                               Description = "4 Buffalo, NE ",
                               Val = "31019",
                           },
                           new GeoFips
                           {
                               Description = "5 Longview, WA (Metropolitan Statistical Area)",
                               Val = "31020",
                           },
                           new GeoFips
                           {
                               Description = "4 Burt, NE ",
                               Val = "31021",
                           },
                           new GeoFips
                           {
                               Description = "4 Butler, NE ",
                               Val = "31023",
                           },
                           new GeoFips
                           {
                               Description = "4 Cass, NE ",
                               Val = "31025",
                           },
                           new GeoFips
                           {
                               Description = "4 Cedar, NE ",
                               Val = "31027",
                           },
                           new GeoFips
                           {
                               Description = "4 Chase, NE ",
                               Val = "31029",
                           },
                           new GeoFips
                           {
                               Description = "4 Cherry, NE ",
                               Val = "31031",
                           },
                           new GeoFips
                           {
                               Description = "4 Cheyenne, NE ",
                               Val = "31033",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, NE ",
                               Val = "31035",
                           },
                           new GeoFips
                           {
                               Description = "4 Colfax, NE ",
                               Val = "31037",
                           },
                           new GeoFips
                           {
                               Description = "4 Cuming, NE ",
                               Val = "31039",
                           },
                           new GeoFips
                           {
                               Description = "4 Custer, NE ",
                               Val = "31041",
                           },
                           new GeoFips
                           {
                               Description = "4 Dakota, NE ",
                               Val = "31043",
                           },
                           new GeoFips
                           {
                               Description = "4 Dawes, NE ",
                               Val = "31045",
                           },
                           new GeoFips
                           {
                               Description = "4 Dawson, NE ",
                               Val = "31047",
                           },
                           new GeoFips
                           {
                               Description = "4 Deuel, NE ",
                               Val = "31049",
                           },
                           new GeoFips
                           {
                               Description = "4 Dixon, NE ",
                               Val = "31051",
                           },
                           new GeoFips
                           {
                               Description = "4 Dodge, NE ",
                               Val = "31053",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, NE ",
                               Val = "31055",
                           },
                           new GeoFips
                           {
                               Description = "4 Dundy, NE ",
                               Val = "31057",
                           },
                           new GeoFips
                           {
                               Description = "4 Fillmore, NE ",
                               Val = "31059",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, NE ",
                               Val = "31061",
                           },
                           new GeoFips
                           {
                               Description = "4 Frontier, NE ",
                               Val = "31063",
                           },
                           new GeoFips
                           {
                               Description = "4 Furnas, NE ",
                               Val = "31065",
                           },
                           new GeoFips
                           {
                               Description = "4 Gage, NE ",
                               Val = "31067",
                           },
                           new GeoFips
                           {
                               Description = "4 Garden, NE ",
                               Val = "31069",
                           },
                           new GeoFips
                           {
                               Description = "4 Garfield, NE ",
                               Val = "31071",
                           },
                           new GeoFips
                           {
                               Description = "4 Gosper, NE ",
                               Val = "31073",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, NE ",
                               Val = "31075",
                           },
                           new GeoFips
                           {
                               Description = "4 Greeley, NE ",
                               Val = "31077",
                           },
                           new GeoFips
                           {
                               Description = "4 Hall, NE ",
                               Val = "31079",
                           },
                           new GeoFips
                           {
                               Description = "5 Los Angeles-Long Beach-Anaheim, CA (Metropolitan Statistical Area)",
                               Val = "31080",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, NE ",
                               Val = "31081",
                           },
                           new GeoFips
                           {
                               Description = "4 Harlan, NE ",
                               Val = "31083",
                           },
                           new GeoFips
                           {
                               Description = "4 Hayes, NE ",
                               Val = "31085",
                           },
                           new GeoFips
                           {
                               Description = "4 Hitchcock, NE ",
                               Val = "31087",
                           },
                           new GeoFips
                           {
                               Description = "4 Holt, NE ",
                               Val = "31089",
                           },
                           new GeoFips
                           {
                               Description = "4 Hooker, NE ",
                               Val = "31091",
                           },
                           new GeoFips
                           {
                               Description = "4 Howard, NE ",
                               Val = "31093",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, NE ",
                               Val = "31095",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, NE ",
                               Val = "31097",
                           },
                           new GeoFips
                           {
                               Description = "4 Kearney, NE ",
                               Val = "31099",
                           },
                           new GeoFips
                           {
                               Description = "4 Keith, NE ",
                               Val = "31101",
                           },
                           new GeoFips
                           {
                               Description = "4 Keya Paha, NE ",
                               Val = "31103",
                           },
                           new GeoFips
                           {
                               Description = "4 Kimball, NE ",
                               Val = "31105",
                           },
                           new GeoFips
                           {
                               Description = "4 Knox, NE ",
                               Val = "31107",
                           },
                           new GeoFips
                           {
                               Description = "4 Lancaster, NE ",
                               Val = "31109",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, NE ",
                               Val = "31111",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, NE ",
                               Val = "31113",
                           },
                           new GeoFips
                           {
                               Description = "4 Loup, NE ",
                               Val = "31115",
                           },
                           new GeoFips
                           {
                               Description = "4 McPherson, NE ",
                               Val = "31117",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, NE ",
                               Val = "31119",
                           },
                           new GeoFips
                           {
                               Description = "4 Merrick, NE ",
                               Val = "31121",
                           },
                           new GeoFips
                           {
                               Description = "4 Morrill, NE ",
                               Val = "31123",
                           },
                           new GeoFips
                           {
                               Description = "4 Nance, NE ",
                               Val = "31125",
                           },
                           new GeoFips
                           {
                               Description = "4 Nemaha, NE ",
                               Val = "31127",
                           },
                           new GeoFips
                           {
                               Description = "4 Nuckolls, NE ",
                               Val = "31129",
                           },
                           new GeoFips
                           {
                               Description = "4 Otoe, NE ",
                               Val = "31131",
                           },
                           new GeoFips
                           {
                               Description = "4 Pawnee, NE ",
                               Val = "31133",
                           },
                           new GeoFips
                           {
                               Description = "4 Perkins, NE ",
                               Val = "31135",
                           },
                           new GeoFips
                           {
                               Description = "4 Phelps, NE ",
                               Val = "31137",
                           },
                           new GeoFips
                           {
                               Description = "4 Pierce, NE ",
                               Val = "31139",
                           },
                           new GeoFips
                           {
                               Description = "5 Louisville/Jefferson County, KY-IN (Metropolitan Statistical Area)",
                               Val = "31140",
                           },
                           new GeoFips
                           {
                               Description = "4 Platte, NE ",
                               Val = "31141",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, NE ",
                               Val = "31143",
                           },
                           new GeoFips
                           {
                               Description = "4 Red Willow, NE ",
                               Val = "31145",
                           },
                           new GeoFips
                           {
                               Description = "4 Richardson, NE ",
                               Val = "31147",
                           },
                           new GeoFips
                           {
                               Description = "4 Rock, NE ",
                               Val = "31149",
                           },
                           new GeoFips
                           {
                               Description = "4 Saline, NE ",
                               Val = "31151",
                           },
                           new GeoFips
                           {
                               Description = "4 Sarpy, NE ",
                               Val = "31153",
                           },
                           new GeoFips
                           {
                               Description = "4 Saunders, NE ",
                               Val = "31155",
                           },
                           new GeoFips
                           {
                               Description = "4 Scotts Bluff, NE ",
                               Val = "31157",
                           },
                           new GeoFips
                           {
                               Description = "4 Seward, NE ",
                               Val = "31159",
                           },
                           new GeoFips
                           {
                               Description = "4 Sheridan, NE ",
                               Val = "31161",
                           },
                           new GeoFips
                           {
                               Description = "4 Sherman, NE ",
                               Val = "31163",
                           },
                           new GeoFips
                           {
                               Description = "4 Sioux, NE ",
                               Val = "31165",
                           },
                           new GeoFips
                           {
                               Description = "4 Stanton, NE ",
                               Val = "31167",
                           },
                           new GeoFips
                           {
                               Description = "4 Thayer, NE ",
                               Val = "31169",
                           },
                           new GeoFips
                           {
                               Description = "4 Thomas, NE ",
                               Val = "31171",
                           },
                           new GeoFips
                           {
                               Description = "4 Thurston, NE ",
                               Val = "31173",
                           },
                           new GeoFips
                           {
                               Description = "4 Valley, NE ",
                               Val = "31175",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, NE ",
                               Val = "31177",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, NE ",
                               Val = "31179",
                           },
                           new GeoFips
                           {
                               Description = "5 Lubbock, TX (Metropolitan Statistical Area)",
                               Val = "31180",
                           },
                           new GeoFips
                           {
                               Description = "4 Webster, NE ",
                               Val = "31181",
                           },
                           new GeoFips
                           {
                               Description = "4 Wheeler, NE ",
                               Val = "31183",
                           },
                           new GeoFips
                           {
                               Description = "4 York, NE ",
                               Val = "31185",
                           },
                           new GeoFips
                           {
                               Description = "5 Lynchburg, VA (Metropolitan Statistical Area)",
                               Val = "31340",
                           },
                           new GeoFips
                           {
                               Description = "5 Macon, GA (Metropolitan Statistical Area)",
                               Val = "31420",
                           },
                           new GeoFips
                           {
                               Description = "5 Madera, CA (Metropolitan Statistical Area)",
                               Val = "31460",
                           },
                           new GeoFips
                           {
                               Description = "5 Madison, WI (Metropolitan Statistical Area)",
                               Val = "31540",
                           },
                           new GeoFips
                           {
                               Description = "5 Manchester-Nashua, NH (Metropolitan Statistical Area)",
                               Val = "31700",
                           },
                           new GeoFips
                           {
                               Description = "5 Manhattan, KS (Metropolitan Statistical Area)",
                               Val = "31740",
                           },
                           new GeoFips
                           {
                               Description = "5 Mankato-North Mankato, MN (Metropolitan Statistical Area)",
                               Val = "31860",
                           },
                           new GeoFips
                           {
                               Description = "5 Mansfield, OH (Metropolitan Statistical Area)",
                               Val = "31900",
                           },
                           new GeoFips
                           {
                               Description = "3 Nevada",
                               Val = "32000",
                           },
                           new GeoFips
                           {
                               Description = "4 Churchill, NV ",
                               Val = "32001",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, NV ",
                               Val = "32003",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, NV ",
                               Val = "32005",
                           },
                           new GeoFips
                           {
                               Description = "4 Elko, NV ",
                               Val = "32007",
                           },
                           new GeoFips
                           {
                               Description = "4 Esmeralda, NV ",
                               Val = "32009",
                           },
                           new GeoFips
                           {
                               Description = "4 Eureka, NV ",
                               Val = "32011",
                           },
                           new GeoFips
                           {
                               Description = "4 Humboldt, NV ",
                               Val = "32013",
                           },
                           new GeoFips
                           {
                               Description = "4 Lander, NV ",
                               Val = "32015",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, NV ",
                               Val = "32017",
                           },
                           new GeoFips
                           {
                               Description = "4 Lyon, NV ",
                               Val = "32019",
                           },
                           new GeoFips
                           {
                               Description = "4 Mineral, NV ",
                               Val = "32021",
                           },
                           new GeoFips
                           {
                               Description = "4 Nye, NV ",
                               Val = "32023",
                           },
                           new GeoFips
                           {
                               Description = "4 Pershing, NV ",
                               Val = "32027",
                           },
                           new GeoFips
                           {
                               Description = "4 Storey, NV ",
                               Val = "32029",
                           },
                           new GeoFips
                           {
                               Description = "4 Washoe, NV ",
                               Val = "32031",
                           },
                           new GeoFips
                           {
                               Description = "4 White Pine, NV ",
                               Val = "32033",
                           },
                           new GeoFips
                           {
                               Description = "4 Carson City (Independent City), NV ",
                               Val = "32510",
                           },
                           new GeoFips
                           {
                               Description = "5 McAllen-Edinburg-Mission, TX (Metropolitan Statistical Area)",
                               Val = "32580",
                           },
                           new GeoFips
                           {
                               Description = "5 Medford, OR (Metropolitan Statistical Area)",
                               Val = "32780",
                           },
                           new GeoFips
                           {
                               Description = "5 Memphis, TN-MS-AR (Metropolitan Statistical Area)",
                               Val = "32820",
                           },
                           new GeoFips
                           {
                               Description = "5 Merced, CA (Metropolitan Statistical Area)",
                               Val = "32900",
                           },
                           new GeoFips
                           {
                               Description = "3 New Hampshire",
                               Val = "33000",
                           },
                           new GeoFips
                           {
                               Description = "4 Belknap, NH ",
                               Val = "33001",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, NH ",
                               Val = "33003",
                           },
                           new GeoFips
                           {
                               Description = "4 Cheshire, NH ",
                               Val = "33005",
                           },
                           new GeoFips
                           {
                               Description = "4 Coos, NH ",
                               Val = "33007",
                           },
                           new GeoFips
                           {
                               Description = "4 Grafton, NH ",
                               Val = "33009",
                           },
                           new GeoFips
                           {
                               Description = "4 Hillsborough, NH ",
                               Val = "33011",
                           },
                           new GeoFips
                           {
                               Description = "4 Merrimack, NH ",
                               Val = "33013",
                           },
                           new GeoFips
                           {
                               Description = "4 Rockingham, NH ",
                               Val = "33015",
                           },
                           new GeoFips
                           {
                               Description = "4 Strafford, NH ",
                               Val = "33017",
                           },
                           new GeoFips
                           {
                               Description = "4 Sullivan, NH ",
                               Val = "33019",
                           },
                           new GeoFips
                           {
                               Description = "5 Miami-Fort Lauderdale-West Palm Beach, FL (Metropolitan Statistical Area)",
                               Val = "33100",
                           },
                           new GeoFips
                           {
                               Description = "5 Michigan City-La Porte, IN (Metropolitan Statistical Area)",
                               Val = "33140",
                           },
                           new GeoFips
                           {
                               Description = "5 Midland, MI (Metropolitan Statistical Area)",
                               Val = "33220",
                           },
                           new GeoFips
                           {
                               Description = "5 Midland, TX (Metropolitan Statistical Area)",
                               Val = "33260",
                           },
                           new GeoFips
                           {
                               Description = "5 Milwaukee-Waukesha-West Allis, WI (Metropolitan Statistical Area)",
                               Val = "33340",
                           },
                           new GeoFips
                           {
                               Description = "5 Minneapolis-St. Paul-Bloomington, MN-WI (Metropolitan Statistical Area)",
                               Val = "33460",
                           },
                           new GeoFips
                           {
                               Description = "5 Missoula, MT (Metropolitan Statistical Area)",
                               Val = "33540",
                           },
                           new GeoFips
                           {
                               Description = "5 Mobile, AL (Metropolitan Statistical Area)",
                               Val = "33660",
                           },
                           new GeoFips
                           {
                               Description = "5 Modesto, CA (Metropolitan Statistical Area)",
                               Val = "33700",
                           },
                           new GeoFips
                           {
                               Description = "5 Monroe, LA (Metropolitan Statistical Area)",
                               Val = "33740",
                           },
                           new GeoFips
                           {
                               Description = "5 Monroe, MI (Metropolitan Statistical Area)",
                               Val = "33780",
                           },
                           new GeoFips
                           {
                               Description = "5 Montgomery, AL (Metropolitan Statistical Area)",
                               Val = "33860",
                           },
                           new GeoFips
                           {
                               Description = "3 New Jersey",
                               Val = "34000",
                           },
                           new GeoFips
                           {
                               Description = "4 Atlantic, NJ ",
                               Val = "34001",
                           },
                           new GeoFips
                           {
                               Description = "4 Bergen, NJ ",
                               Val = "34003",
                           },
                           new GeoFips
                           {
                               Description = "4 Burlington, NJ ",
                               Val = "34005",
                           },
                           new GeoFips
                           {
                               Description = "4 Camden, NJ ",
                               Val = "34007",
                           },
                           new GeoFips
                           {
                               Description = "4 Cape May, NJ ",
                               Val = "34009",
                           },
                           new GeoFips
                           {
                               Description = "4 Cumberland, NJ ",
                               Val = "34011",
                           },
                           new GeoFips
                           {
                               Description = "4 Essex, NJ ",
                               Val = "34013",
                           },
                           new GeoFips
                           {
                               Description = "4 Gloucester, NJ ",
                               Val = "34015",
                           },
                           new GeoFips
                           {
                               Description = "4 Hudson, NJ ",
                               Val = "34017",
                           },
                           new GeoFips
                           {
                               Description = "4 Hunterdon, NJ ",
                               Val = "34019",
                           },
                           new GeoFips
                           {
                               Description = "4 Mercer, NJ ",
                               Val = "34021",
                           },
                           new GeoFips
                           {
                               Description = "4 Middlesex, NJ ",
                               Val = "34023",
                           },
                           new GeoFips
                           {
                               Description = "4 Monmouth, NJ ",
                               Val = "34025",
                           },
                           new GeoFips
                           {
                               Description = "4 Morris, NJ ",
                               Val = "34027",
                           },
                           new GeoFips
                           {
                               Description = "4 Ocean, NJ ",
                               Val = "34029",
                           },
                           new GeoFips
                           {
                               Description = "4 Passaic, NJ ",
                               Val = "34031",
                           },
                           new GeoFips
                           {
                               Description = "4 Salem, NJ ",
                               Val = "34033",
                           },
                           new GeoFips
                           {
                               Description = "4 Somerset, NJ ",
                               Val = "34035",
                           },
                           new GeoFips
                           {
                               Description = "4 Sussex, NJ ",
                               Val = "34037",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, NJ ",
                               Val = "34039",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, NJ ",
                               Val = "34041",
                           },
                           new GeoFips
                           {
                               Description = "5 Morgantown, WV (Metropolitan Statistical Area)",
                               Val = "34060",
                           },
                           new GeoFips
                           {
                               Description = "5 Morristown, TN (Metropolitan Statistical Area)",
                               Val = "34100",
                           },
                           new GeoFips
                           {
                               Description = "5 Mount Vernon-Anacortes, WA (Metropolitan Statistical Area)",
                               Val = "34580",
                           },
                           new GeoFips
                           {
                               Description = "5 Muncie, IN (Metropolitan Statistical Area)",
                               Val = "34620",
                           },
                           new GeoFips
                           {
                               Description = "5 Muskegon, MI (Metropolitan Statistical Area)",
                               Val = "34740",
                           },
                           new GeoFips
                           {
                               Description = "5 Myrtle Beach-Conway-North Myrtle Beach, SC-NC (Metropolitan Statistical Area)",
                               Val = "34820",
                           },
                           new GeoFips
                           {
                               Description = "5 Napa, CA (Metropolitan Statistical Area)",
                               Val = "34900",
                           },
                           new GeoFips
                           {
                               Description = "5 Naples-Immokalee-Marco Island, FL (Metropolitan Statistical Area)",
                               Val = "34940",
                           },
                           new GeoFips
                           {
                               Description = "5 Nashville-Davidson--Murfreesboro--Franklin, TN (Metropolitan Statistical Area)",
                               Val = "34980",
                           },
                           new GeoFips
                           {
                               Description = "3 New Mexico",
                               Val = "35000",
                           },
                           new GeoFips
                           {
                               Description = "4 Bernalillo, NM ",
                               Val = "35001",
                           },
                           new GeoFips
                           {
                               Description = "4 Catron, NM ",
                               Val = "35003",
                           },
                           new GeoFips
                           {
                               Description = "4 Chaves, NM ",
                               Val = "35005",
                           },
                           new GeoFips
                           {
                               Description = "4 Cibola, NM ",
                               Val = "35006",
                           },
                           new GeoFips
                           {
                               Description = "4 Colfax, NM ",
                               Val = "35007",
                           },
                           new GeoFips
                           {
                               Description = "4 Curry, NM ",
                               Val = "35009",
                           },
                           new GeoFips
                           {
                               Description = "4 De Baca, NM ",
                               Val = "35011",
                           },
                           new GeoFips
                           {
                               Description = "4 Do�a Ana, NM ",
                               Val = "35013",
                           },
                           new GeoFips
                           {
                               Description = "4 Eddy, NM ",
                               Val = "35015",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, NM ",
                               Val = "35017",
                           },
                           new GeoFips
                           {
                               Description = "4 Guadalupe, NM ",
                               Val = "35019",
                           },
                           new GeoFips
                           {
                               Description = "4 Harding, NM ",
                               Val = "35021",
                           },
                           new GeoFips
                           {
                               Description = "4 Hidalgo, NM ",
                               Val = "35023",
                           },
                           new GeoFips
                           {
                               Description = "4 Lea, NM ",
                               Val = "35025",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, NM ",
                               Val = "35027",
                           },
                           new GeoFips
                           {
                               Description = "4 Los Alamos, NM ",
                               Val = "35028",
                           },
                           new GeoFips
                           {
                               Description = "4 Luna, NM ",
                               Val = "35029",
                           },
                           new GeoFips
                           {
                               Description = "4 McKinley, NM ",
                               Val = "35031",
                           },
                           new GeoFips
                           {
                               Description = "4 Mora, NM ",
                               Val = "35033",
                           },
                           new GeoFips
                           {
                               Description = "4 Otero, NM ",
                               Val = "35035",
                           },
                           new GeoFips
                           {
                               Description = "4 Quay, NM ",
                               Val = "35037",
                           },
                           new GeoFips
                           {
                               Description = "4 Rio Arriba, NM ",
                               Val = "35039",
                           },
                           new GeoFips
                           {
                               Description = "4 Roosevelt, NM ",
                               Val = "35041",
                           },
                           new GeoFips
                           {
                               Description = "4 Sandoval, NM ",
                               Val = "35043",
                           },
                           new GeoFips
                           {
                               Description = "4 San Juan, NM ",
                               Val = "35045",
                           },
                           new GeoFips
                           {
                               Description = "4 San Miguel, NM ",
                               Val = "35047",
                           },
                           new GeoFips
                           {
                               Description = "4 Santa Fe, NM ",
                               Val = "35049",
                           },
                           new GeoFips
                           {
                               Description = "4 Sierra, NM ",
                               Val = "35051",
                           },
                           new GeoFips
                           {
                               Description = "4 Socorro, NM ",
                               Val = "35053",
                           },
                           new GeoFips
                           {
                               Description = "4 Taos, NM ",
                               Val = "35055",
                           },
                           new GeoFips
                           {
                               Description = "4 Torrance, NM ",
                               Val = "35057",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, NM ",
                               Val = "35059",
                           },
                           new GeoFips
                           {
                               Description = "4 Valencia, NM ",
                               Val = "35061",
                           },
                           new GeoFips
                           {
                               Description = "5 New Bern, NC (Metropolitan Statistical Area)",
                               Val = "35100",
                           },
                           new GeoFips
                           {
                               Description = "5 New Haven-Milford, CT (Metropolitan Statistical Area)",
                               Val = "35300",
                           },
                           new GeoFips
                           {
                               Description = "5 New Orleans-Metairie, LA (Metropolitan Statistical Area)",
                               Val = "35380",
                           },
                           new GeoFips
                           {
                               Description = "5 New York-Newark-Jersey City, NY-NJ-PA (Metropolitan Statistical Area)",
                               Val = "35620",
                           },
                           new GeoFips
                           {
                               Description = "5 Niles-Benton Harbor, MI (Metropolitan Statistical Area)",
                               Val = "35660",
                           },
                           new GeoFips
                           {
                               Description = "5 North Port-Sarasota-Bradenton, FL (Metropolitan Statistical Area)",
                               Val = "35840",
                           },
                           new GeoFips
                           {
                               Description = "5 Norwich-New London, CT (Metropolitan Statistical Area)",
                               Val = "35980",
                           },
                           new GeoFips
                           {
                               Description = "3 New York",
                               Val = "36000",
                           },
                           new GeoFips
                           {
                               Description = "4 Albany, NY ",
                               Val = "36001",
                           },
                           new GeoFips
                           {
                               Description = "4 Allegany, NY ",
                               Val = "36003",
                           },
                           new GeoFips
                           {
                               Description = "4 Bronx, NY ",
                               Val = "36005",
                           },
                           new GeoFips
                           {
                               Description = "4 Broome, NY ",
                               Val = "36007",
                           },
                           new GeoFips
                           {
                               Description = "4 Cattaraugus, NY ",
                               Val = "36009",
                           },
                           new GeoFips
                           {
                               Description = "4 Cayuga, NY ",
                               Val = "36011",
                           },
                           new GeoFips
                           {
                               Description = "4 Chautauqua, NY ",
                               Val = "36013",
                           },
                           new GeoFips
                           {
                               Description = "4 Chemung, NY ",
                               Val = "36015",
                           },
                           new GeoFips
                           {
                               Description = "4 Chenango, NY ",
                               Val = "36017",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinton, NY ",
                               Val = "36019",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbia, NY ",
                               Val = "36021",
                           },
                           new GeoFips
                           {
                               Description = "4 Cortland, NY ",
                               Val = "36023",
                           },
                           new GeoFips
                           {
                               Description = "4 Delaware, NY ",
                               Val = "36025",
                           },
                           new GeoFips
                           {
                               Description = "4 Dutchess, NY ",
                               Val = "36027",
                           },
                           new GeoFips
                           {
                               Description = "4 Erie, NY ",
                               Val = "36029",
                           },
                           new GeoFips
                           {
                               Description = "4 Essex, NY ",
                               Val = "36031",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, NY ",
                               Val = "36033",
                           },
                           new GeoFips
                           {
                               Description = "4 Fulton, NY ",
                               Val = "36035",
                           },
                           new GeoFips
                           {
                               Description = "4 Genesee, NY ",
                               Val = "36037",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, NY ",
                               Val = "36039",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, NY ",
                               Val = "36041",
                           },
                           new GeoFips
                           {
                               Description = "4 Herkimer, NY ",
                               Val = "36043",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, NY ",
                               Val = "36045",
                           },
                           new GeoFips
                           {
                               Description = "4 Kings, NY ",
                               Val = "36047",
                           },
                           new GeoFips
                           {
                               Description = "4 Lewis, NY ",
                               Val = "36049",
                           },
                           new GeoFips
                           {
                               Description = "4 Livingston, NY ",
                               Val = "36051",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, NY ",
                               Val = "36053",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, NY ",
                               Val = "36055",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, NY ",
                               Val = "36057",
                           },
                           new GeoFips
                           {
                               Description = "4 Nassau, NY ",
                               Val = "36059",
                           },
                           new GeoFips
                           {
                               Description = "4 New York, NY ",
                               Val = "36061",
                           },
                           new GeoFips
                           {
                               Description = "4 Niagara, NY ",
                               Val = "36063",
                           },
                           new GeoFips
                           {
                               Description = "4 Oneida, NY ",
                               Val = "36065",
                           },
                           new GeoFips
                           {
                               Description = "4 Onondaga, NY ",
                               Val = "36067",
                           },
                           new GeoFips
                           {
                               Description = "4 Ontario, NY ",
                               Val = "36069",
                           },
                           new GeoFips
                           {
                               Description = "4 Orange, NY ",
                               Val = "36071",
                           },
                           new GeoFips
                           {
                               Description = "4 Orleans, NY ",
                               Val = "36073",
                           },
                           new GeoFips
                           {
                               Description = "4 Oswego, NY ",
                               Val = "36075",
                           },
                           new GeoFips
                           {
                               Description = "4 Otsego, NY ",
                               Val = "36077",
                           },
                           new GeoFips
                           {
                               Description = "4 Putnam, NY ",
                               Val = "36079",
                           },
                           new GeoFips
                           {
                               Description = "4 Queens, NY ",
                               Val = "36081",
                           },
                           new GeoFips
                           {
                               Description = "4 Rensselaer, NY ",
                               Val = "36083",
                           },
                           new GeoFips
                           {
                               Description = "4 Richmond, NY ",
                               Val = "36085",
                           },
                           new GeoFips
                           {
                               Description = "4 Rockland, NY ",
                               Val = "36087",
                           },
                           new GeoFips
                           {
                               Description = "4 St. Lawrence, NY ",
                               Val = "36089",
                           },
                           new GeoFips
                           {
                               Description = "4 Saratoga, NY ",
                               Val = "36091",
                           },
                           new GeoFips
                           {
                               Description = "4 Schenectady, NY ",
                               Val = "36093",
                           },
                           new GeoFips
                           {
                               Description = "4 Schoharie, NY ",
                               Val = "36095",
                           },
                           new GeoFips
                           {
                               Description = "4 Schuyler, NY ",
                               Val = "36097",
                           },
                           new GeoFips
                           {
                               Description = "4 Seneca, NY ",
                               Val = "36099",
                           },
                           new GeoFips
                           {
                               Description = "5 Ocala, FL (Metropolitan Statistical Area)",
                               Val = "36100",
                           },
                           new GeoFips
                           {
                               Description = "4 Steuben, NY ",
                               Val = "36101",
                           },
                           new GeoFips
                           {
                               Description = "4 Suffolk, NY ",
                               Val = "36103",
                           },
                           new GeoFips
                           {
                               Description = "4 Sullivan, NY ",
                               Val = "36105",
                           },
                           new GeoFips
                           {
                               Description = "4 Tioga, NY ",
                               Val = "36107",
                           },
                           new GeoFips
                           {
                               Description = "4 Tompkins, NY ",
                               Val = "36109",
                           },
                           new GeoFips
                           {
                               Description = "4 Ulster, NY ",
                               Val = "36111",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, NY ",
                               Val = "36113",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, NY ",
                               Val = "36115",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, NY ",
                               Val = "36117",
                           },
                           new GeoFips
                           {
                               Description = "4 Westchester, NY ",
                               Val = "36119",
                           },
                           new GeoFips
                           {
                               Description = "4 Wyoming, NY ",
                               Val = "36121",
                           },
                           new GeoFips
                           {
                               Description = "4 Yates, NY ",
                               Val = "36123",
                           },
                           new GeoFips
                           {
                               Description = "5 Ocean City, NJ (Metropolitan Statistical Area)",
                               Val = "36140",
                           },
                           new GeoFips
                           {
                               Description = "5 Odessa, TX (Metropolitan Statistical Area)",
                               Val = "36220",
                           },
                           new GeoFips
                           {
                               Description = "5 Ogden-Clearfield, UT (Metropolitan Statistical Area)",
                               Val = "36260",
                           },
                           new GeoFips
                           {
                               Description = "5 Oklahoma City, OK (Metropolitan Statistical Area)",
                               Val = "36420",
                           },
                           new GeoFips
                           {
                               Description = "5 Olympia-Tumwater, WA (Metropolitan Statistical Area)",
                               Val = "36500",
                           },
                           new GeoFips
                           {
                               Description = "5 Omaha-Council Bluffs, NE-IA (Metropolitan Statistical Area)",
                               Val = "36540",
                           },
                           new GeoFips
                           {
                               Description = "5 Orlando-Kissimmee-Sanford, FL (Metropolitan Statistical Area)",
                               Val = "36740",
                           },
                           new GeoFips
                           {
                               Description = "5 Oshkosh-Neenah, WI (Metropolitan Statistical Area)",
                               Val = "36780",
                           },
                           new GeoFips
                           {
                               Description = "5 Owensboro, KY (Metropolitan Statistical Area)",
                               Val = "36980",
                           },
                           new GeoFips
                           {
                               Description = "3 North Carolina",
                               Val = "37000",
                           },
                           new GeoFips
                           {
                               Description = "4 Alamance, NC ",
                               Val = "37001",
                           },
                           new GeoFips
                           {
                               Description = "4 Alexander, NC ",
                               Val = "37003",
                           },
                           new GeoFips
                           {
                               Description = "4 Alleghany, NC ",
                               Val = "37005",
                           },
                           new GeoFips
                           {
                               Description = "4 Anson, NC ",
                               Val = "37007",
                           },
                           new GeoFips
                           {
                               Description = "4 Ashe, NC ",
                               Val = "37009",
                           },
                           new GeoFips
                           {
                               Description = "4 Avery, NC ",
                               Val = "37011",
                           },
                           new GeoFips
                           {
                               Description = "4 Beaufort, NC ",
                               Val = "37013",
                           },
                           new GeoFips
                           {
                               Description = "4 Bertie, NC ",
                               Val = "37015",
                           },
                           new GeoFips
                           {
                               Description = "4 Bladen, NC ",
                               Val = "37017",
                           },
                           new GeoFips
                           {
                               Description = "4 Brunswick, NC ",
                               Val = "37019",
                           },
                           new GeoFips
                           {
                               Description = "4 Buncombe, NC ",
                               Val = "37021",
                           },
                           new GeoFips
                           {
                               Description = "4 Burke, NC ",
                               Val = "37023",
                           },
                           new GeoFips
                           {
                               Description = "4 Cabarrus, NC ",
                               Val = "37025",
                           },
                           new GeoFips
                           {
                               Description = "4 Caldwell, NC ",
                               Val = "37027",
                           },
                           new GeoFips
                           {
                               Description = "4 Camden, NC ",
                               Val = "37029",
                           },
                           new GeoFips
                           {
                               Description = "4 Carteret, NC ",
                               Val = "37031",
                           },
                           new GeoFips
                           {
                               Description = "4 Caswell, NC ",
                               Val = "37033",
                           },
                           new GeoFips
                           {
                               Description = "4 Catawba, NC ",
                               Val = "37035",
                           },
                           new GeoFips
                           {
                               Description = "4 Chatham, NC ",
                               Val = "37037",
                           },
                           new GeoFips
                           {
                               Description = "4 Cherokee, NC ",
                               Val = "37039",
                           },
                           new GeoFips
                           {
                               Description = "4 Chowan, NC ",
                               Val = "37041",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, NC ",
                               Val = "37043",
                           },
                           new GeoFips
                           {
                               Description = "4 Cleveland, NC ",
                               Val = "37045",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbus, NC ",
                               Val = "37047",
                           },
                           new GeoFips
                           {
                               Description = "4 Craven, NC ",
                               Val = "37049",
                           },
                           new GeoFips
                           {
                               Description = "4 Cumberland, NC ",
                               Val = "37051",
                           },
                           new GeoFips
                           {
                               Description = "4 Currituck, NC ",
                               Val = "37053",
                           },
                           new GeoFips
                           {
                               Description = "4 Dare, NC ",
                               Val = "37055",
                           },
                           new GeoFips
                           {
                               Description = "4 Davidson, NC ",
                               Val = "37057",
                           },
                           new GeoFips
                           {
                               Description = "4 Davie, NC ",
                               Val = "37059",
                           },
                           new GeoFips
                           {
                               Description = "4 Duplin, NC ",
                               Val = "37061",
                           },
                           new GeoFips
                           {
                               Description = "4 Durham, NC ",
                               Val = "37063",
                           },
                           new GeoFips
                           {
                               Description = "4 Edgecombe, NC ",
                               Val = "37065",
                           },
                           new GeoFips
                           {
                               Description = "4 Forsyth, NC ",
                               Val = "37067",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, NC ",
                               Val = "37069",
                           },
                           new GeoFips
                           {
                               Description = "4 Gaston, NC ",
                               Val = "37071",
                           },
                           new GeoFips
                           {
                               Description = "4 Gates, NC ",
                               Val = "37073",
                           },
                           new GeoFips
                           {
                               Description = "4 Graham, NC ",
                               Val = "37075",
                           },
                           new GeoFips
                           {
                               Description = "4 Granville, NC ",
                               Val = "37077",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, NC ",
                               Val = "37079",
                           },
                           new GeoFips
                           {
                               Description = "4 Guilford, NC ",
                               Val = "37081",
                           },
                           new GeoFips
                           {
                               Description = "4 Halifax, NC ",
                               Val = "37083",
                           },
                           new GeoFips
                           {
                               Description = "4 Harnett, NC ",
                               Val = "37085",
                           },
                           new GeoFips
                           {
                               Description = "4 Haywood, NC ",
                               Val = "37087",
                           },
                           new GeoFips
                           {
                               Description = "4 Henderson, NC ",
                               Val = "37089",
                           },
                           new GeoFips
                           {
                               Description = "4 Hertford, NC ",
                               Val = "37091",
                           },
                           new GeoFips
                           {
                               Description = "4 Hoke, NC ",
                               Val = "37093",
                           },
                           new GeoFips
                           {
                               Description = "4 Hyde, NC ",
                               Val = "37095",
                           },
                           new GeoFips
                           {
                               Description = "4 Iredell, NC ",
                               Val = "37097",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, NC ",
                               Val = "37099",
                           },
                           new GeoFips
                           {
                               Description = "5 Oxnard-Thousand Oaks-Ventura, CA (Metropolitan Statistical Area)",
                               Val = "37100",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnston, NC ",
                               Val = "37101",
                           },
                           new GeoFips
                           {
                               Description = "4 Jones, NC ",
                               Val = "37103",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, NC ",
                               Val = "37105",
                           },
                           new GeoFips
                           {
                               Description = "4 Lenoir, NC ",
                               Val = "37107",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, NC ",
                               Val = "37109",
                           },
                           new GeoFips
                           {
                               Description = "4 McDowell, NC ",
                               Val = "37111",
                           },
                           new GeoFips
                           {
                               Description = "4 Macon, NC ",
                               Val = "37113",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, NC ",
                               Val = "37115",
                           },
                           new GeoFips
                           {
                               Description = "4 Martin, NC ",
                               Val = "37117",
                           },
                           new GeoFips
                           {
                               Description = "4 Mecklenburg, NC ",
                               Val = "37119",
                           },
                           new GeoFips
                           {
                               Description = "4 Mitchell, NC ",
                               Val = "37121",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, NC ",
                               Val = "37123",
                           },
                           new GeoFips
                           {
                               Description = "4 Moore, NC ",
                               Val = "37125",
                           },
                           new GeoFips
                           {
                               Description = "4 Nash, NC ",
                               Val = "37127",
                           },
                           new GeoFips
                           {
                               Description = "4 New Hanover, NC ",
                               Val = "37129",
                           },
                           new GeoFips
                           {
                               Description = "4 Northampton, NC ",
                               Val = "37131",
                           },
                           new GeoFips
                           {
                               Description = "4 Onslow, NC ",
                               Val = "37133",
                           },
                           new GeoFips
                           {
                               Description = "4 Orange, NC ",
                               Val = "37135",
                           },
                           new GeoFips
                           {
                               Description = "4 Pamlico, NC ",
                               Val = "37137",
                           },
                           new GeoFips
                           {
                               Description = "4 Pasquotank, NC ",
                               Val = "37139",
                           },
                           new GeoFips
                           {
                               Description = "4 Pender, NC ",
                               Val = "37141",
                           },
                           new GeoFips
                           {
                               Description = "4 Perquimans, NC ",
                               Val = "37143",
                           },
                           new GeoFips
                           {
                               Description = "4 Person, NC ",
                               Val = "37145",
                           },
                           new GeoFips
                           {
                               Description = "4 Pitt, NC ",
                               Val = "37147",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, NC ",
                               Val = "37149",
                           },
                           new GeoFips
                           {
                               Description = "4 Randolph, NC ",
                               Val = "37151",
                           },
                           new GeoFips
                           {
                               Description = "4 Richmond, NC ",
                               Val = "37153",
                           },
                           new GeoFips
                           {
                               Description = "4 Robeson, NC ",
                               Val = "37155",
                           },
                           new GeoFips
                           {
                               Description = "4 Rockingham, NC ",
                               Val = "37157",
                           },
                           new GeoFips
                           {
                               Description = "4 Rowan, NC ",
                               Val = "37159",
                           },
                           new GeoFips
                           {
                               Description = "4 Rutherford, NC ",
                               Val = "37161",
                           },
                           new GeoFips
                           {
                               Description = "4 Sampson, NC ",
                               Val = "37163",
                           },
                           new GeoFips
                           {
                               Description = "4 Scotland, NC ",
                               Val = "37165",
                           },
                           new GeoFips
                           {
                               Description = "4 Stanly, NC ",
                               Val = "37167",
                           },
                           new GeoFips
                           {
                               Description = "4 Stokes, NC ",
                               Val = "37169",
                           },
                           new GeoFips
                           {
                               Description = "4 Surry, NC ",
                               Val = "37171",
                           },
                           new GeoFips
                           {
                               Description = "4 Swain, NC ",
                               Val = "37173",
                           },
                           new GeoFips
                           {
                               Description = "4 Transylvania, NC ",
                               Val = "37175",
                           },
                           new GeoFips
                           {
                               Description = "4 Tyrrell, NC ",
                               Val = "37177",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, NC ",
                               Val = "37179",
                           },
                           new GeoFips
                           {
                               Description = "4 Vance, NC ",
                               Val = "37181",
                           },
                           new GeoFips
                           {
                               Description = "4 Wake, NC ",
                               Val = "37183",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, NC ",
                               Val = "37185",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, NC ",
                               Val = "37187",
                           },
                           new GeoFips
                           {
                               Description = "4 Watauga, NC ",
                               Val = "37189",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, NC ",
                               Val = "37191",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilkes, NC ",
                               Val = "37193",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilson, NC ",
                               Val = "37195",
                           },
                           new GeoFips
                           {
                               Description = "4 Yadkin, NC ",
                               Val = "37197",
                           },
                           new GeoFips
                           {
                               Description = "4 Yancey, NC ",
                               Val = "37199",
                           },
                           new GeoFips
                           {
                               Description = "5 Palm Bay-Melbourne-Titusville, FL (Metropolitan Statistical Area)",
                               Val = "37340",
                           },
                           new GeoFips
                           {
                               Description = "5 Panama City, FL (Metropolitan Statistical Area)",
                               Val = "37460",
                           },
                           new GeoFips
                           {
                               Description = "5 Parkersburg-Vienna, WV (Metropolitan Statistical Area)",
                               Val = "37620",
                           },
                           new GeoFips
                           {
                               Description = "5 Pensacola-Ferry Pass-Brent, FL (Metropolitan Statistical Area)",
                               Val = "37860",
                           },
                           new GeoFips
                           {
                               Description = "5 Peoria, IL (Metropolitan Statistical Area)",
                               Val = "37900",
                           },
                           new GeoFips
                           {
                               Description = "5 Philadelphia-Camden-Wilmington, PA-NJ-DE-MD (Metropolitan Statistical Area)",
                               Val = "37980",
                           },
                           new GeoFips
                           {
                               Description = "3 North Dakota",
                               Val = "38000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, ND ",
                               Val = "38001",
                           },
                           new GeoFips
                           {
                               Description = "4 Barnes, ND ",
                               Val = "38003",
                           },
                           new GeoFips
                           {
                               Description = "4 Benson, ND ",
                               Val = "38005",
                           },
                           new GeoFips
                           {
                               Description = "4 Billings, ND ",
                               Val = "38007",
                           },
                           new GeoFips
                           {
                               Description = "4 Bottineau, ND ",
                               Val = "38009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bowman, ND ",
                               Val = "38011",
                           },
                           new GeoFips
                           {
                               Description = "4 Burke, ND ",
                               Val = "38013",
                           },
                           new GeoFips
                           {
                               Description = "4 Burleigh, ND ",
                               Val = "38015",
                           },
                           new GeoFips
                           {
                               Description = "4 Cass, ND ",
                               Val = "38017",
                           },
                           new GeoFips
                           {
                               Description = "4 Cavalier, ND ",
                               Val = "38019",
                           },
                           new GeoFips
                           {
                               Description = "4 Dickey, ND ",
                               Val = "38021",
                           },
                           new GeoFips
                           {
                               Description = "4 Divide, ND ",
                               Val = "38023",
                           },
                           new GeoFips
                           {
                               Description = "4 Dunn, ND ",
                               Val = "38025",
                           },
                           new GeoFips
                           {
                               Description = "4 Eddy, ND ",
                               Val = "38027",
                           },
                           new GeoFips
                           {
                               Description = "4 Emmons, ND ",
                               Val = "38029",
                           },
                           new GeoFips
                           {
                               Description = "4 Foster, ND ",
                               Val = "38031",
                           },
                           new GeoFips
                           {
                               Description = "4 Golden Valley, ND ",
                               Val = "38033",
                           },
                           new GeoFips
                           {
                               Description = "4 Grand Forks, ND ",
                               Val = "38035",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, ND ",
                               Val = "38037",
                           },
                           new GeoFips
                           {
                               Description = "4 Griggs, ND ",
                               Val = "38039",
                           },
                           new GeoFips
                           {
                               Description = "4 Hettinger, ND ",
                               Val = "38041",
                           },
                           new GeoFips
                           {
                               Description = "4 Kidder, ND ",
                               Val = "38043",
                           },
                           new GeoFips
                           {
                               Description = "4 LaMoure, ND ",
                               Val = "38045",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, ND ",
                               Val = "38047",
                           },
                           new GeoFips
                           {
                               Description = "4 McHenry, ND ",
                               Val = "38049",
                           },
                           new GeoFips
                           {
                               Description = "4 McIntosh, ND ",
                               Val = "38051",
                           },
                           new GeoFips
                           {
                               Description = "4 McKenzie, ND ",
                               Val = "38053",
                           },
                           new GeoFips
                           {
                               Description = "4 McLean, ND ",
                               Val = "38055",
                           },
                           new GeoFips
                           {
                               Description = "4 Mercer, ND ",
                               Val = "38057",
                           },
                           new GeoFips
                           {
                               Description = "4 Morton, ND ",
                               Val = "38059",
                           },
                           new GeoFips
                           {
                               Description = "5 Phoenix-Mesa-Scottsdale, AZ (Metropolitan Statistical Area)",
                               Val = "38060",
                           },
                           new GeoFips
                           {
                               Description = "4 Mountrail, ND ",
                               Val = "38061",
                           },
                           new GeoFips
                           {
                               Description = "4 Nelson, ND ",
                               Val = "38063",
                           },
                           new GeoFips
                           {
                               Description = "4 Oliver, ND ",
                               Val = "38065",
                           },
                           new GeoFips
                           {
                               Description = "4 Pembina, ND ",
                               Val = "38067",
                           },
                           new GeoFips
                           {
                               Description = "4 Pierce, ND ",
                               Val = "38069",
                           },
                           new GeoFips
                           {
                               Description = "4 Ramsey, ND ",
                               Val = "38071",
                           },
                           new GeoFips
                           {
                               Description = "4 Ransom, ND ",
                               Val = "38073",
                           },
                           new GeoFips
                           {
                               Description = "4 Renville, ND ",
                               Val = "38075",
                           },
                           new GeoFips
                           {
                               Description = "4 Richland, ND ",
                               Val = "38077",
                           },
                           new GeoFips
                           {
                               Description = "4 Rolette, ND ",
                               Val = "38079",
                           },
                           new GeoFips
                           {
                               Description = "4 Sargent, ND ",
                               Val = "38081",
                           },
                           new GeoFips
                           {
                               Description = "4 Sheridan, ND ",
                               Val = "38083",
                           },
                           new GeoFips
                           {
                               Description = "4 Sioux, ND ",
                               Val = "38085",
                           },
                           new GeoFips
                           {
                               Description = "4 Slope, ND ",
                               Val = "38087",
                           },
                           new GeoFips
                           {
                               Description = "4 Stark, ND ",
                               Val = "38089",
                           },
                           new GeoFips
                           {
                               Description = "4 Steele, ND ",
                               Val = "38091",
                           },
                           new GeoFips
                           {
                               Description = "4 Stutsman, ND ",
                               Val = "38093",
                           },
                           new GeoFips
                           {
                               Description = "4 Towner, ND ",
                               Val = "38095",
                           },
                           new GeoFips
                           {
                               Description = "4 Traill, ND ",
                               Val = "38097",
                           },
                           new GeoFips
                           {
                               Description = "4 Walsh, ND ",
                               Val = "38099",
                           },
                           new GeoFips
                           {
                               Description = "4 Ward, ND ",
                               Val = "38101",
                           },
                           new GeoFips
                           {
                               Description = "4 Wells, ND ",
                               Val = "38103",
                           },
                           new GeoFips
                           {
                               Description = "4 Williams, ND ",
                               Val = "38105",
                           },
                           new GeoFips
                           {
                               Description = "5 Pine Bluff, AR (Metropolitan Statistical Area)",
                               Val = "38220",
                           },
                           new GeoFips
                           {
                               Description = "5 Pittsburgh, PA (Metropolitan Statistical Area)",
                               Val = "38300",
                           },
                           new GeoFips
                           {
                               Description = "5 Pittsfield, MA (Metropolitan Statistical Area)",
                               Val = "38340",
                           },
                           new GeoFips
                           {
                               Description = "5 Pocatello, ID (Metropolitan Statistical Area)",
                               Val = "38540",
                           },
                           new GeoFips
                           {
                               Description = "5 Portland-South Portland, ME (Metropolitan Statistical Area)",
                               Val = "38860",
                           },
                           new GeoFips
                           {
                               Description = "5 Portland-Vancouver-Hillsboro, OR-WA (Metropolitan Statistical Area)",
                               Val = "38900",
                           },
                           new GeoFips
                           {
                               Description = "5 Port St. Lucie, FL (Metropolitan Statistical Area)",
                               Val = "38940",
                           },
                           new GeoFips
                           {
                               Description = "3 Ohio",
                               Val = "39000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, OH ",
                               Val = "39001",
                           },
                           new GeoFips
                           {
                               Description = "4 Allen, OH ",
                               Val = "39003",
                           },
                           new GeoFips
                           {
                               Description = "4 Ashland, OH ",
                               Val = "39005",
                           },
                           new GeoFips
                           {
                               Description = "4 Ashtabula, OH ",
                               Val = "39007",
                           },
                           new GeoFips
                           {
                               Description = "4 Athens, OH ",
                               Val = "39009",
                           },
                           new GeoFips
                           {
                               Description = "4 Auglaize, OH ",
                               Val = "39011",
                           },
                           new GeoFips
                           {
                               Description = "4 Belmont, OH ",
                               Val = "39013",
                           },
                           new GeoFips
                           {
                               Description = "4 Brown, OH ",
                               Val = "39015",
                           },
                           new GeoFips
                           {
                               Description = "4 Butler, OH ",
                               Val = "39017",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, OH ",
                               Val = "39019",
                           },
                           new GeoFips
                           {
                               Description = "4 Champaign, OH ",
                               Val = "39021",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, OH ",
                               Val = "39023",
                           },
                           new GeoFips
                           {
                               Description = "4 Clermont, OH ",
                               Val = "39025",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinton, OH ",
                               Val = "39027",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbiana, OH ",
                               Val = "39029",
                           },
                           new GeoFips
                           {
                               Description = "4 Coshocton, OH ",
                               Val = "39031",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, OH ",
                               Val = "39033",
                           },
                           new GeoFips
                           {
                               Description = "4 Cuyahoga, OH ",
                               Val = "39035",
                           },
                           new GeoFips
                           {
                               Description = "4 Darke, OH ",
                               Val = "39037",
                           },
                           new GeoFips
                           {
                               Description = "4 Defiance, OH ",
                               Val = "39039",
                           },
                           new GeoFips
                           {
                               Description = "4 Delaware, OH ",
                               Val = "39041",
                           },
                           new GeoFips
                           {
                               Description = "4 Erie, OH ",
                               Val = "39043",
                           },
                           new GeoFips
                           {
                               Description = "4 Fairfield, OH ",
                               Val = "39045",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, OH ",
                               Val = "39047",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, OH ",
                               Val = "39049",
                           },
                           new GeoFips
                           {
                               Description = "4 Fulton, OH ",
                               Val = "39051",
                           },
                           new GeoFips
                           {
                               Description = "4 Gallia, OH ",
                               Val = "39053",
                           },
                           new GeoFips
                           {
                               Description = "4 Geauga, OH ",
                               Val = "39055",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, OH ",
                               Val = "39057",
                           },
                           new GeoFips
                           {
                               Description = "4 Guernsey, OH ",
                               Val = "39059",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, OH ",
                               Val = "39061",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, OH ",
                               Val = "39063",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardin, OH ",
                               Val = "39065",
                           },
                           new GeoFips
                           {
                               Description = "4 Harrison, OH ",
                               Val = "39067",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry, OH ",
                               Val = "39069",
                           },
                           new GeoFips
                           {
                               Description = "4 Highland, OH ",
                               Val = "39071",
                           },
                           new GeoFips
                           {
                               Description = "4 Hocking, OH ",
                               Val = "39073",
                           },
                           new GeoFips
                           {
                               Description = "4 Holmes, OH ",
                               Val = "39075",
                           },
                           new GeoFips
                           {
                               Description = "4 Huron, OH ",
                               Val = "39077",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, OH ",
                               Val = "39079",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, OH ",
                               Val = "39081",
                           },
                           new GeoFips
                           {
                               Description = "4 Knox, OH ",
                               Val = "39083",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, OH ",
                               Val = "39085",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, OH ",
                               Val = "39087",
                           },
                           new GeoFips
                           {
                               Description = "4 Licking, OH ",
                               Val = "39089",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, OH ",
                               Val = "39091",
                           },
                           new GeoFips
                           {
                               Description = "4 Lorain, OH ",
                               Val = "39093",
                           },
                           new GeoFips
                           {
                               Description = "4 Lucas, OH ",
                               Val = "39095",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, OH ",
                               Val = "39097",
                           },
                           new GeoFips
                           {
                               Description = "4 Mahoning, OH ",
                               Val = "39099",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, OH ",
                               Val = "39101",
                           },
                           new GeoFips
                           {
                               Description = "4 Medina, OH ",
                               Val = "39103",
                           },
                           new GeoFips
                           {
                               Description = "4 Meigs, OH ",
                               Val = "39105",
                           },
                           new GeoFips
                           {
                               Description = "4 Mercer, OH ",
                               Val = "39107",
                           },
                           new GeoFips
                           {
                               Description = "4 Miami, OH ",
                               Val = "39109",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, OH ",
                               Val = "39111",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, OH ",
                               Val = "39113",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, OH ",
                               Val = "39115",
                           },
                           new GeoFips
                           {
                               Description = "4 Morrow, OH ",
                               Val = "39117",
                           },
                           new GeoFips
                           {
                               Description = "4 Muskingum, OH ",
                               Val = "39119",
                           },
                           new GeoFips
                           {
                               Description = "4 Noble, OH ",
                               Val = "39121",
                           },
                           new GeoFips
                           {
                               Description = "4 Ottawa, OH ",
                               Val = "39123",
                           },
                           new GeoFips
                           {
                               Description = "4 Paulding, OH ",
                               Val = "39125",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, OH ",
                               Val = "39127",
                           },
                           new GeoFips
                           {
                               Description = "4 Pickaway, OH ",
                               Val = "39129",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, OH ",
                               Val = "39131",
                           },
                           new GeoFips
                           {
                               Description = "4 Portage, OH ",
                               Val = "39133",
                           },
                           new GeoFips
                           {
                               Description = "4 Preble, OH ",
                               Val = "39135",
                           },
                           new GeoFips
                           {
                               Description = "4 Putnam, OH ",
                               Val = "39137",
                           },
                           new GeoFips
                           {
                               Description = "4 Richland, OH ",
                               Val = "39139",
                           },
                           new GeoFips
                           {
                               Description = "5 Prescott, AZ (Metropolitan Statistical Area)",
                               Val = "39140",
                           },
                           new GeoFips
                           {
                               Description = "4 Ross, OH ",
                               Val = "39141",
                           },
                           new GeoFips
                           {
                               Description = "4 Sandusky, OH ",
                               Val = "39143",
                           },
                           new GeoFips
                           {
                               Description = "4 Scioto, OH ",
                               Val = "39145",
                           },
                           new GeoFips
                           {
                               Description = "4 Seneca, OH ",
                               Val = "39147",
                           },
                           new GeoFips
                           {
                               Description = "4 Shelby, OH ",
                               Val = "39149",
                           },
                           new GeoFips
                           {
                               Description = "4 Stark, OH ",
                               Val = "39151",
                           },
                           new GeoFips
                           {
                               Description = "4 Summit, OH ",
                               Val = "39153",
                           },
                           new GeoFips
                           {
                               Description = "4 Trumbull, OH ",
                               Val = "39155",
                           },
                           new GeoFips
                           {
                               Description = "4 Tuscarawas, OH ",
                               Val = "39157",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, OH ",
                               Val = "39159",
                           },
                           new GeoFips
                           {
                               Description = "4 Van Wert, OH ",
                               Val = "39161",
                           },
                           new GeoFips
                           {
                               Description = "4 Vinton, OH ",
                               Val = "39163",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, OH ",
                               Val = "39165",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, OH ",
                               Val = "39167",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, OH ",
                               Val = "39169",
                           },
                           new GeoFips
                           {
                               Description = "4 Williams, OH ",
                               Val = "39171",
                           },
                           new GeoFips
                           {
                               Description = "4 Wood, OH ",
                               Val = "39173",
                           },
                           new GeoFips
                           {
                               Description = "4 Wyandot, OH ",
                               Val = "39175",
                           },
                           new GeoFips
                           {
                               Description = "5 Providence-Warwick, RI-MA (Metropolitan Statistical Area)",
                               Val = "39300",
                           },
                           new GeoFips
                           {
                               Description = "5 Provo-Orem, UT (Metropolitan Statistical Area)",
                               Val = "39340",
                           },
                           new GeoFips
                           {
                               Description = "5 Pueblo, CO (Metropolitan Statistical Area)",
                               Val = "39380",
                           },
                           new GeoFips
                           {
                               Description = "5 Punta Gorda, FL (Metropolitan Statistical Area)",
                               Val = "39460",
                           },
                           new GeoFips
                           {
                               Description = "5 Racine, WI (Metropolitan Statistical Area)",
                               Val = "39540",
                           },
                           new GeoFips
                           {
                               Description = "5 Raleigh, NC (Metropolitan Statistical Area)",
                               Val = "39580",
                           },
                           new GeoFips
                           {
                               Description = "5 Rapid City, SD (Metropolitan Statistical Area)",
                               Val = "39660",
                           },
                           new GeoFips
                           {
                               Description = "5 Reading, PA (Metropolitan Statistical Area)",
                               Val = "39740",
                           },
                           new GeoFips
                           {
                               Description = "5 Redding, CA (Metropolitan Statistical Area)",
                               Val = "39820",
                           },
                           new GeoFips
                           {
                               Description = "5 Reno, NV (Metropolitan Statistical Area)",
                               Val = "39900",
                           },
                           new GeoFips
                           {
                               Description = "3 Oklahoma",
                               Val = "40000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adair, OK ",
                               Val = "40001",
                           },
                           new GeoFips
                           {
                               Description = "4 Alfalfa, OK ",
                               Val = "40003",
                           },
                           new GeoFips
                           {
                               Description = "4 Atoka, OK ",
                               Val = "40005",
                           },
                           new GeoFips
                           {
                               Description = "4 Beaver, OK ",
                               Val = "40007",
                           },
                           new GeoFips
                           {
                               Description = "4 Beckham, OK ",
                               Val = "40009",
                           },
                           new GeoFips
                           {
                               Description = "4 Blaine, OK ",
                               Val = "40011",
                           },
                           new GeoFips
                           {
                               Description = "4 Bryan, OK ",
                               Val = "40013",
                           },
                           new GeoFips
                           {
                               Description = "4 Caddo, OK ",
                               Val = "40015",
                           },
                           new GeoFips
                           {
                               Description = "4 Canadian, OK ",
                               Val = "40017",
                           },
                           new GeoFips
                           {
                               Description = "4 Carter, OK ",
                               Val = "40019",
                           },
                           new GeoFips
                           {
                               Description = "4 Cherokee, OK ",
                               Val = "40021",
                           },
                           new GeoFips
                           {
                               Description = "4 Choctaw, OK ",
                               Val = "40023",
                           },
                           new GeoFips
                           {
                               Description = "4 Cimarron, OK ",
                               Val = "40025",
                           },
                           new GeoFips
                           {
                               Description = "4 Cleveland, OK ",
                               Val = "40027",
                           },
                           new GeoFips
                           {
                               Description = "4 Coal, OK ",
                               Val = "40029",
                           },
                           new GeoFips
                           {
                               Description = "4 Comanche, OK ",
                               Val = "40031",
                           },
                           new GeoFips
                           {
                               Description = "4 Cotton, OK ",
                               Val = "40033",
                           },
                           new GeoFips
                           {
                               Description = "4 Craig, OK ",
                               Val = "40035",
                           },
                           new GeoFips
                           {
                               Description = "4 Creek, OK ",
                               Val = "40037",
                           },
                           new GeoFips
                           {
                               Description = "4 Custer, OK ",
                               Val = "40039",
                           },
                           new GeoFips
                           {
                               Description = "4 Delaware, OK ",
                               Val = "40041",
                           },
                           new GeoFips
                           {
                               Description = "4 Dewey, OK ",
                               Val = "40043",
                           },
                           new GeoFips
                           {
                               Description = "4 Ellis, OK ",
                               Val = "40045",
                           },
                           new GeoFips
                           {
                               Description = "4 Garfield, OK ",
                               Val = "40047",
                           },
                           new GeoFips
                           {
                               Description = "4 Garvin, OK ",
                               Val = "40049",
                           },
                           new GeoFips
                           {
                               Description = "4 Grady, OK ",
                               Val = "40051",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, OK ",
                               Val = "40053",
                           },
                           new GeoFips
                           {
                               Description = "4 Greer, OK ",
                               Val = "40055",
                           },
                           new GeoFips
                           {
                               Description = "4 Harmon, OK ",
                               Val = "40057",
                           },
                           new GeoFips
                           {
                               Description = "4 Harper, OK ",
                               Val = "40059",
                           },
                           new GeoFips
                           {
                               Description = "5 Richmond, VA (Metropolitan Statistical Area)",
                               Val = "40060",
                           },
                           new GeoFips
                           {
                               Description = "4 Haskell, OK ",
                               Val = "40061",
                           },
                           new GeoFips
                           {
                               Description = "4 Hughes, OK ",
                               Val = "40063",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, OK ",
                               Val = "40065",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, OK ",
                               Val = "40067",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnston, OK ",
                               Val = "40069",
                           },
                           new GeoFips
                           {
                               Description = "4 Kay, OK ",
                               Val = "40071",
                           },
                           new GeoFips
                           {
                               Description = "4 Kingfisher, OK ",
                               Val = "40073",
                           },
                           new GeoFips
                           {
                               Description = "4 Kiowa, OK ",
                               Val = "40075",
                           },
                           new GeoFips
                           {
                               Description = "4 Latimer, OK ",
                               Val = "40077",
                           },
                           new GeoFips
                           {
                               Description = "4 Le Flore, OK ",
                               Val = "40079",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, OK ",
                               Val = "40081",
                           },
                           new GeoFips
                           {
                               Description = "4 Logan, OK ",
                               Val = "40083",
                           },
                           new GeoFips
                           {
                               Description = "4 Love, OK ",
                               Val = "40085",
                           },
                           new GeoFips
                           {
                               Description = "4 McClain, OK ",
                               Val = "40087",
                           },
                           new GeoFips
                           {
                               Description = "4 McCurtain, OK ",
                               Val = "40089",
                           },
                           new GeoFips
                           {
                               Description = "4 McIntosh, OK ",
                               Val = "40091",
                           },
                           new GeoFips
                           {
                               Description = "4 Major, OK ",
                               Val = "40093",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, OK ",
                               Val = "40095",
                           },
                           new GeoFips
                           {
                               Description = "4 Mayes, OK ",
                               Val = "40097",
                           },
                           new GeoFips
                           {
                               Description = "4 Murray, OK ",
                               Val = "40099",
                           },
                           new GeoFips
                           {
                               Description = "4 Muskogee, OK ",
                               Val = "40101",
                           },
                           new GeoFips
                           {
                               Description = "4 Noble, OK ",
                               Val = "40103",
                           },
                           new GeoFips
                           {
                               Description = "4 Nowata, OK ",
                               Val = "40105",
                           },
                           new GeoFips
                           {
                               Description = "4 Okfuskee, OK ",
                               Val = "40107",
                           },
                           new GeoFips
                           {
                               Description = "4 Oklahoma, OK ",
                               Val = "40109",
                           },
                           new GeoFips
                           {
                               Description = "4 Okmulgee, OK ",
                               Val = "40111",
                           },
                           new GeoFips
                           {
                               Description = "4 Osage, OK ",
                               Val = "40113",
                           },
                           new GeoFips
                           {
                               Description = "4 Ottawa, OK ",
                               Val = "40115",
                           },
                           new GeoFips
                           {
                               Description = "4 Pawnee, OK ",
                               Val = "40117",
                           },
                           new GeoFips
                           {
                               Description = "4 Payne, OK ",
                               Val = "40119",
                           },
                           new GeoFips
                           {
                               Description = "4 Pittsburg, OK ",
                               Val = "40121",
                           },
                           new GeoFips
                           {
                               Description = "4 Pontotoc, OK ",
                               Val = "40123",
                           },
                           new GeoFips
                           {
                               Description = "4 Pottawatomie, OK ",
                               Val = "40125",
                           },
                           new GeoFips
                           {
                               Description = "4 Pushmataha, OK ",
                               Val = "40127",
                           },
                           new GeoFips
                           {
                               Description = "4 Roger Mills, OK ",
                               Val = "40129",
                           },
                           new GeoFips
                           {
                               Description = "4 Rogers, OK ",
                               Val = "40131",
                           },
                           new GeoFips
                           {
                               Description = "4 Seminole, OK ",
                               Val = "40133",
                           },
                           new GeoFips
                           {
                               Description = "4 Sequoyah, OK ",
                               Val = "40135",
                           },
                           new GeoFips
                           {
                               Description = "4 Stephens, OK ",
                               Val = "40137",
                           },
                           new GeoFips
                           {
                               Description = "4 Texas, OK ",
                               Val = "40139",
                           },
                           new GeoFips
                           {
                               Description = "5 Riverside-San Bernardino-Ontario, CA (Metropolitan Statistical Area)",
                               Val = "40140",
                           },
                           new GeoFips
                           {
                               Description = "4 Tillman, OK ",
                               Val = "40141",
                           },
                           new GeoFips
                           {
                               Description = "4 Tulsa, OK ",
                               Val = "40143",
                           },
                           new GeoFips
                           {
                               Description = "4 Wagoner, OK ",
                               Val = "40145",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, OK ",
                               Val = "40147",
                           },
                           new GeoFips
                           {
                               Description = "4 Washita, OK ",
                               Val = "40149",
                           },
                           new GeoFips
                           {
                               Description = "4 Woods, OK ",
                               Val = "40151",
                           },
                           new GeoFips
                           {
                               Description = "4 Woodward, OK ",
                               Val = "40153",
                           },
                           new GeoFips
                           {
                               Description = "5 Roanoke, VA (Metropolitan Statistical Area)",
                               Val = "40220",
                           },
                           new GeoFips
                           {
                               Description = "5 Rochester, MN (Metropolitan Statistical Area)",
                               Val = "40340",
                           },
                           new GeoFips
                           {
                               Description = "5 Rochester, NY (Metropolitan Statistical Area)",
                               Val = "40380",
                           },
                           new GeoFips
                           {
                               Description = "5 Rockford, IL (Metropolitan Statistical Area)",
                               Val = "40420",
                           },
                           new GeoFips
                           {
                               Description = "5 Rocky Mount, NC (Metropolitan Statistical Area)",
                               Val = "40580",
                           },
                           new GeoFips
                           {
                               Description = "5 Rome, GA (Metropolitan Statistical Area)",
                               Val = "40660",
                           },
                           new GeoFips
                           {
                               Description = "5 Sacramento--Roseville--Arden-Arcade, CA (Metropolitan Statistical Area)",
                               Val = "40900",
                           },
                           new GeoFips
                           {
                               Description = "5 Saginaw, MI (Metropolitan Statistical Area)",
                               Val = "40980",
                           },
                           new GeoFips
                           {
                               Description = "3 Oregon",
                               Val = "41000",
                           },
                           new GeoFips
                           {
                               Description = "4 Baker, OR ",
                               Val = "41001",
                           },
                           new GeoFips
                           {
                               Description = "4 Benton, OR ",
                               Val = "41003",
                           },
                           new GeoFips
                           {
                               Description = "4 Clackamas, OR ",
                               Val = "41005",
                           },
                           new GeoFips
                           {
                               Description = "4 Clatsop, OR ",
                               Val = "41007",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbia, OR ",
                               Val = "41009",
                           },
                           new GeoFips
                           {
                               Description = "4 Coos, OR ",
                               Val = "41011",
                           },
                           new GeoFips
                           {
                               Description = "4 Crook, OR ",
                               Val = "41013",
                           },
                           new GeoFips
                           {
                               Description = "4 Curry, OR ",
                               Val = "41015",
                           },
                           new GeoFips
                           {
                               Description = "4 Deschutes, OR ",
                               Val = "41017",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, OR ",
                               Val = "41019",
                           },
                           new GeoFips
                           {
                               Description = "4 Gilliam, OR ",
                               Val = "41021",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, OR ",
                               Val = "41023",
                           },
                           new GeoFips
                           {
                               Description = "4 Harney, OR ",
                               Val = "41025",
                           },
                           new GeoFips
                           {
                               Description = "4 Hood River, OR ",
                               Val = "41027",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, OR ",
                               Val = "41029",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, OR ",
                               Val = "41031",
                           },
                           new GeoFips
                           {
                               Description = "4 Josephine, OR ",
                               Val = "41033",
                           },
                           new GeoFips
                           {
                               Description = "4 Klamath, OR ",
                               Val = "41035",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, OR ",
                               Val = "41037",
                           },
                           new GeoFips
                           {
                               Description = "4 Lane, OR ",
                               Val = "41039",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, OR ",
                               Val = "41041",
                           },
                           new GeoFips
                           {
                               Description = "4 Linn, OR ",
                               Val = "41043",
                           },
                           new GeoFips
                           {
                               Description = "4 Malheur, OR ",
                               Val = "41045",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, OR ",
                               Val = "41047",
                           },
                           new GeoFips
                           {
                               Description = "4 Morrow, OR ",
                               Val = "41049",
                           },
                           new GeoFips
                           {
                               Description = "4 Multnomah, OR ",
                               Val = "41051",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, OR ",
                               Val = "41053",
                           },
                           new GeoFips
                           {
                               Description = "4 Sherman, OR ",
                               Val = "41055",
                           },
                           new GeoFips
                           {
                               Description = "4 Tillamook, OR ",
                               Val = "41057",
                           },
                           new GeoFips
                           {
                               Description = "4 Umatilla, OR ",
                               Val = "41059",
                           },
                           new GeoFips
                           {
                               Description = "5 St. Cloud, MN (Metropolitan Statistical Area)",
                               Val = "41060",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, OR ",
                               Val = "41061",
                           },
                           new GeoFips
                           {
                               Description = "4 Wallowa, OR ",
                               Val = "41063",
                           },
                           new GeoFips
                           {
                               Description = "4 Wasco, OR ",
                               Val = "41065",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, OR ",
                               Val = "41067",
                           },
                           new GeoFips
                           {
                               Description = "4 Wheeler, OR ",
                               Val = "41069",
                           },
                           new GeoFips
                           {
                               Description = "4 Yamhill, OR ",
                               Val = "41071",
                           },
                           new GeoFips
                           {
                               Description = "5 St. George, UT (Metropolitan Statistical Area)",
                               Val = "41100",
                           },
                           new GeoFips
                           {
                               Description = "5 St. Joseph, MO-KS (Metropolitan Statistical Area)",
                               Val = "41140",
                           },
                           new GeoFips
                           {
                               Description = "5 St. Louis, MO-IL (Metropolitan Statistical Area)",
                               Val = "41180",
                           },
                           new GeoFips
                           {
                               Description = "5 Salem, OR (Metropolitan Statistical Area)",
                               Val = "41420",
                           },
                           new GeoFips
                           {
                               Description = "5 Salinas, CA (Metropolitan Statistical Area)",
                               Val = "41500",
                           },
                           new GeoFips
                           {
                               Description = "5 Salisbury, MD-DE (Metropolitan Statistical Area)",
                               Val = "41540",
                           },
                           new GeoFips
                           {
                               Description = "5 Salt Lake City, UT (Metropolitan Statistical Area)",
                               Val = "41620",
                           },
                           new GeoFips
                           {
                               Description = "5 San Angelo, TX (Metropolitan Statistical Area)",
                               Val = "41660",
                           },
                           new GeoFips
                           {
                               Description = "5 San Antonio-New Braunfels, TX (Metropolitan Statistical Area)",
                               Val = "41700",
                           },
                           new GeoFips
                           {
                               Description = "5 San Diego-Carlsbad, CA (Metropolitan Statistical Area)",
                               Val = "41740",
                           },
                           new GeoFips
                           {
                               Description = "5 San Francisco-Oakland-Hayward, CA (Metropolitan Statistical Area)",
                               Val = "41860",
                           },
                           new GeoFips
                           {
                               Description = "5 San Jose-Sunnyvale-Santa Clara, CA (Metropolitan Statistical Area)",
                               Val = "41940",
                           },
                           new GeoFips
                           {
                               Description = "3 Pennsylvania",
                               Val = "42000",
                           },
                           new GeoFips
                           {
                               Description = "4 Adams, PA ",
                               Val = "42001",
                           },
                           new GeoFips
                           {
                               Description = "4 Allegheny, PA ",
                               Val = "42003",
                           },
                           new GeoFips
                           {
                               Description = "4 Armstrong, PA ",
                               Val = "42005",
                           },
                           new GeoFips
                           {
                               Description = "4 Beaver, PA ",
                               Val = "42007",
                           },
                           new GeoFips
                           {
                               Description = "4 Bedford, PA ",
                               Val = "42009",
                           },
                           new GeoFips
                           {
                               Description = "4 Berks, PA ",
                               Val = "42011",
                           },
                           new GeoFips
                           {
                               Description = "4 Blair, PA ",
                               Val = "42013",
                           },
                           new GeoFips
                           {
                               Description = "4 Bradford, PA ",
                               Val = "42015",
                           },
                           new GeoFips
                           {
                               Description = "4 Bucks, PA ",
                               Val = "42017",
                           },
                           new GeoFips
                           {
                               Description = "4 Butler, PA ",
                               Val = "42019",
                           },
                           new GeoFips
                           {
                               Description = "5 San Luis Obispo-Paso Robles-Arroyo Grande, CA (Metropolitan Statistical Area)",
                               Val = "42020",
                           },
                           new GeoFips
                           {
                               Description = "4 Cambria, PA ",
                               Val = "42021",
                           },
                           new GeoFips
                           {
                               Description = "4 Cameron, PA ",
                               Val = "42023",
                           },
                           new GeoFips
                           {
                               Description = "4 Carbon, PA ",
                               Val = "42025",
                           },
                           new GeoFips
                           {
                               Description = "4 Centre, PA ",
                               Val = "42027",
                           },
                           new GeoFips
                           {
                               Description = "4 Chester, PA ",
                               Val = "42029",
                           },
                           new GeoFips
                           {
                               Description = "4 Clarion, PA ",
                               Val = "42031",
                           },
                           new GeoFips
                           {
                               Description = "4 Clearfield, PA ",
                               Val = "42033",
                           },
                           new GeoFips
                           {
                               Description = "4 Clinton, PA ",
                               Val = "42035",
                           },
                           new GeoFips
                           {
                               Description = "4 Columbia, PA ",
                               Val = "42037",
                           },
                           new GeoFips
                           {
                               Description = "4 Crawford, PA ",
                               Val = "42039",
                           },
                           new GeoFips
                           {
                               Description = "4 Cumberland, PA ",
                               Val = "42041",
                           },
                           new GeoFips
                           {
                               Description = "4 Dauphin, PA ",
                               Val = "42043",
                           },
                           new GeoFips
                           {
                               Description = "4 Delaware, PA ",
                               Val = "42045",
                           },
                           new GeoFips
                           {
                               Description = "4 Elk, PA ",
                               Val = "42047",
                           },
                           new GeoFips
                           {
                               Description = "4 Erie, PA ",
                               Val = "42049",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, PA ",
                               Val = "42051",
                           },
                           new GeoFips
                           {
                               Description = "4 Forest, PA ",
                               Val = "42053",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, PA ",
                               Val = "42055",
                           },
                           new GeoFips
                           {
                               Description = "4 Fulton, PA ",
                               Val = "42057",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, PA ",
                               Val = "42059",
                           },
                           new GeoFips
                           {
                               Description = "4 Huntingdon, PA ",
                               Val = "42061",
                           },
                           new GeoFips
                           {
                               Description = "4 Indiana, PA ",
                               Val = "42063",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, PA ",
                               Val = "42065",
                           },
                           new GeoFips
                           {
                               Description = "4 Juniata, PA ",
                               Val = "42067",
                           },
                           new GeoFips
                           {
                               Description = "4 Lackawanna, PA ",
                               Val = "42069",
                           },
                           new GeoFips
                           {
                               Description = "4 Lancaster, PA ",
                               Val = "42071",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, PA ",
                               Val = "42073",
                           },
                           new GeoFips
                           {
                               Description = "4 Lebanon, PA ",
                               Val = "42075",
                           },
                           new GeoFips
                           {
                               Description = "4 Lehigh, PA ",
                               Val = "42077",
                           },
                           new GeoFips
                           {
                               Description = "4 Luzerne, PA ",
                               Val = "42079",
                           },
                           new GeoFips
                           {
                               Description = "4 Lycoming, PA ",
                               Val = "42081",
                           },
                           new GeoFips
                           {
                               Description = "4 McKean, PA ",
                               Val = "42083",
                           },
                           new GeoFips
                           {
                               Description = "4 Mercer, PA ",
                               Val = "42085",
                           },
                           new GeoFips
                           {
                               Description = "4 Mifflin, PA ",
                               Val = "42087",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, PA ",
                               Val = "42089",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, PA ",
                               Val = "42091",
                           },
                           new GeoFips
                           {
                               Description = "4 Montour, PA ",
                               Val = "42093",
                           },
                           new GeoFips
                           {
                               Description = "4 Northampton, PA ",
                               Val = "42095",
                           },
                           new GeoFips
                           {
                               Description = "4 Northumberland, PA ",
                               Val = "42097",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, PA ",
                               Val = "42099",
                           },
                           new GeoFips
                           {
                               Description = "5 Santa Cruz-Watsonville, CA (Metropolitan Statistical Area)",
                               Val = "42100",
                           },
                           new GeoFips
                           {
                               Description = "4 Philadelphia, PA ",
                               Val = "42101",
                           },
                           new GeoFips
                           {
                               Description = "4 Pike, PA ",
                               Val = "42103",
                           },
                           new GeoFips
                           {
                               Description = "4 Potter, PA ",
                               Val = "42105",
                           },
                           new GeoFips
                           {
                               Description = "4 Schuylkill, PA ",
                               Val = "42107",
                           },
                           new GeoFips
                           {
                               Description = "4 Snyder, PA ",
                               Val = "42109",
                           },
                           new GeoFips
                           {
                               Description = "4 Somerset, PA ",
                               Val = "42111",
                           },
                           new GeoFips
                           {
                               Description = "4 Sullivan, PA ",
                               Val = "42113",
                           },
                           new GeoFips
                           {
                               Description = "4 Susquehanna, PA ",
                               Val = "42115",
                           },
                           new GeoFips
                           {
                               Description = "4 Tioga, PA ",
                               Val = "42117",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, PA ",
                               Val = "42119",
                           },
                           new GeoFips
                           {
                               Description = "4 Venango, PA ",
                               Val = "42121",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, PA ",
                               Val = "42123",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, PA ",
                               Val = "42125",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, PA ",
                               Val = "42127",
                           },
                           new GeoFips
                           {
                               Description = "4 Westmoreland, PA ",
                               Val = "42129",
                           },
                           new GeoFips
                           {
                               Description = "4 Wyoming, PA ",
                               Val = "42131",
                           },
                           new GeoFips
                           {
                               Description = "4 York, PA ",
                               Val = "42133",
                           },
                           new GeoFips
                           {
                               Description = "5 Santa Fe, NM (Metropolitan Statistical Area)",
                               Val = "42140",
                           },
                           new GeoFips
                           {
                               Description = "5 Santa Maria-Santa Barbara, CA (Metropolitan Statistical Area)",
                               Val = "42200",
                           },
                           new GeoFips
                           {
                               Description = "5 Santa Rosa, CA (Metropolitan Statistical Area)",
                               Val = "42220",
                           },
                           new GeoFips
                           {
                               Description = "5 Savannah, GA (Metropolitan Statistical Area)",
                               Val = "42340",
                           },
                           new GeoFips
                           {
                               Description = "5 Scranton--Wilkes-Barre--Hazleton, PA (Metropolitan Statistical Area)",
                               Val = "42540",
                           },
                           new GeoFips
                           {
                               Description = "5 Seattle-Tacoma-Bellevue, WA (Metropolitan Statistical Area)",
                               Val = "42660",
                           },
                           new GeoFips
                           {
                               Description = "5 Sebastian-Vero Beach, FL (Metropolitan Statistical Area)",
                               Val = "42680",
                           },
                           new GeoFips
                           {
                               Description = "5 Sebring, FL (Metropolitan Statistical Area)",
                               Val = "42700",
                           },
                           new GeoFips
                           {
                               Description = "5 Sheboygan, WI (Metropolitan Statistical Area)",
                               Val = "43100",
                           },
                           new GeoFips
                           {
                               Description = "5 Sherman-Denison, TX (Metropolitan Statistical Area)",
                               Val = "43300",
                           },
                           new GeoFips
                           {
                               Description = "5 Shreveport-Bossier City, LA (Metropolitan Statistical Area)",
                               Val = "43340",
                           },
                           new GeoFips
                           {
                               Description = "5 Sierra Vista-Douglas, AZ (Metropolitan Statistical Area)",
                               Val = "43420",
                           },
                           new GeoFips
                           {
                               Description = "5 Sioux City, IA-NE-SD (Metropolitan Statistical Area)",
                               Val = "43580",
                           },
                           new GeoFips
                           {
                               Description = "5 Sioux Falls, SD (Metropolitan Statistical Area)",
                               Val = "43620",
                           },
                           new GeoFips
                           {
                               Description = "5 South Bend-Mishawaka, IN-MI (Metropolitan Statistical Area)",
                               Val = "43780",
                           },
                           new GeoFips
                           {
                               Description = "5 Spartanburg, SC (Metropolitan Statistical Area)",
                               Val = "43900",
                           },
                           new GeoFips
                           {
                               Description = "3 Rhode Island",
                               Val = "44000",
                           },
                           new GeoFips
                           {
                               Description = "4 Bristol, RI ",
                               Val = "44001",
                           },
                           new GeoFips
                           {
                               Description = "4 Kent, RI ",
                               Val = "44003",
                           },
                           new GeoFips
                           {
                               Description = "4 Newport, RI ",
                               Val = "44005",
                           },
                           new GeoFips
                           {
                               Description = "4 Providence, RI ",
                               Val = "44007",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, RI ",
                               Val = "44009",
                           },
                           new GeoFips
                           {
                               Description = "5 Spokane-Spokane Valley, WA (Metropolitan Statistical Area)",
                               Val = "44060",
                           },
                           new GeoFips
                           {
                               Description = "5 Springfield, IL (Metropolitan Statistical Area)",
                               Val = "44100",
                           },
                           new GeoFips
                           {
                               Description = "5 Springfield, MA (Metropolitan Statistical Area)",
                               Val = "44140",
                           },
                           new GeoFips
                           {
                               Description = "5 Springfield, MO (Metropolitan Statistical Area)",
                               Val = "44180",
                           },
                           new GeoFips
                           {
                               Description = "5 Springfield, OH (Metropolitan Statistical Area)",
                               Val = "44220",
                           },
                           new GeoFips
                           {
                               Description = "5 State College, PA (Metropolitan Statistical Area)",
                               Val = "44300",
                           },
                           new GeoFips
                           {
                               Description = "5 Staunton-Waynesboro, VA (Metropolitan Statistical Area)",
                               Val = "44420",
                           },
                           new GeoFips
                           {
                               Description = "5 Stockton-Lodi, CA (Metropolitan Statistical Area)",
                               Val = "44700",
                           },
                           new GeoFips
                           {
                               Description = "5 Sumter, SC (Metropolitan Statistical Area)",
                               Val = "44940",
                           },
                           new GeoFips
                           {
                               Description = "3 South Carolina",
                               Val = "45000",
                           },
                           new GeoFips
                           {
                               Description = "4 Abbeville, SC ",
                               Val = "45001",
                           },
                           new GeoFips
                           {
                               Description = "4 Aiken, SC ",
                               Val = "45003",
                           },
                           new GeoFips
                           {
                               Description = "4 Allendale, SC ",
                               Val = "45005",
                           },
                           new GeoFips
                           {
                               Description = "4 Anderson, SC ",
                               Val = "45007",
                           },
                           new GeoFips
                           {
                               Description = "4 Bamberg, SC ",
                               Val = "45009",
                           },
                           new GeoFips
                           {
                               Description = "4 Barnwell, SC ",
                               Val = "45011",
                           },
                           new GeoFips
                           {
                               Description = "4 Beaufort, SC ",
                               Val = "45013",
                           },
                           new GeoFips
                           {
                               Description = "4 Berkeley, SC ",
                               Val = "45015",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, SC ",
                               Val = "45017",
                           },
                           new GeoFips
                           {
                               Description = "4 Charleston, SC ",
                               Val = "45019",
                           },
                           new GeoFips
                           {
                               Description = "4 Cherokee, SC ",
                               Val = "45021",
                           },
                           new GeoFips
                           {
                               Description = "4 Chester, SC ",
                               Val = "45023",
                           },
                           new GeoFips
                           {
                               Description = "4 Chesterfield, SC ",
                               Val = "45025",
                           },
                           new GeoFips
                           {
                               Description = "4 Clarendon, SC ",
                               Val = "45027",
                           },
                           new GeoFips
                           {
                               Description = "4 Colleton, SC ",
                               Val = "45029",
                           },
                           new GeoFips
                           {
                               Description = "4 Darlington, SC ",
                               Val = "45031",
                           },
                           new GeoFips
                           {
                               Description = "4 Dillon, SC ",
                               Val = "45033",
                           },
                           new GeoFips
                           {
                               Description = "4 Dorchester, SC ",
                               Val = "45035",
                           },
                           new GeoFips
                           {
                               Description = "4 Edgefield, SC ",
                               Val = "45037",
                           },
                           new GeoFips
                           {
                               Description = "4 Fairfield, SC ",
                               Val = "45039",
                           },
                           new GeoFips
                           {
                               Description = "4 Florence, SC ",
                               Val = "45041",
                           },
                           new GeoFips
                           {
                               Description = "4 Georgetown, SC ",
                               Val = "45043",
                           },
                           new GeoFips
                           {
                               Description = "4 Greenville, SC ",
                               Val = "45045",
                           },
                           new GeoFips
                           {
                               Description = "4 Greenwood, SC ",
                               Val = "45047",
                           },
                           new GeoFips
                           {
                               Description = "4 Hampton, SC ",
                               Val = "45049",
                           },
                           new GeoFips
                           {
                               Description = "4 Horry, SC ",
                               Val = "45051",
                           },
                           new GeoFips
                           {
                               Description = "4 Jasper, SC ",
                               Val = "45053",
                           },
                           new GeoFips
                           {
                               Description = "4 Kershaw, SC ",
                               Val = "45055",
                           },
                           new GeoFips
                           {
                               Description = "4 Lancaster, SC ",
                               Val = "45057",
                           },
                           new GeoFips
                           {
                               Description = "4 Laurens, SC ",
                               Val = "45059",
                           },
                           new GeoFips
                           {
                               Description = "5 Syracuse, NY (Metropolitan Statistical Area)",
                               Val = "45060",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, SC ",
                               Val = "45061",
                           },
                           new GeoFips
                           {
                               Description = "4 Lexington, SC ",
                               Val = "45063",
                           },
                           new GeoFips
                           {
                               Description = "4 McCormick, SC ",
                               Val = "45065",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, SC ",
                               Val = "45067",
                           },
                           new GeoFips
                           {
                               Description = "4 Marlboro, SC ",
                               Val = "45069",
                           },
                           new GeoFips
                           {
                               Description = "4 Newberry, SC ",
                               Val = "45071",
                           },
                           new GeoFips
                           {
                               Description = "4 Oconee, SC ",
                               Val = "45073",
                           },
                           new GeoFips
                           {
                               Description = "4 Orangeburg, SC ",
                               Val = "45075",
                           },
                           new GeoFips
                           {
                               Description = "4 Pickens, SC ",
                               Val = "45077",
                           },
                           new GeoFips
                           {
                               Description = "4 Richland, SC ",
                               Val = "45079",
                           },
                           new GeoFips
                           {
                               Description = "4 Saluda, SC ",
                               Val = "45081",
                           },
                           new GeoFips
                           {
                               Description = "4 Spartanburg, SC ",
                               Val = "45083",
                           },
                           new GeoFips
                           {
                               Description = "4 Sumter, SC ",
                               Val = "45085",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, SC ",
                               Val = "45087",
                           },
                           new GeoFips
                           {
                               Description = "4 Williamsburg, SC ",
                               Val = "45089",
                           },
                           new GeoFips
                           {
                               Description = "4 York, SC ",
                               Val = "45091",
                           },
                           new GeoFips
                           {
                               Description = "5 Tallahassee, FL (Metropolitan Statistical Area)",
                               Val = "45220",
                           },
                           new GeoFips
                           {
                               Description = "5 Tampa-St. Petersburg-Clearwater, FL (Metropolitan Statistical Area)",
                               Val = "45300",
                           },
                           new GeoFips
                           {
                               Description = "5 Terre Haute, IN (Metropolitan Statistical Area)",
                               Val = "45460",
                           },
                           new GeoFips
                           {
                               Description = "5 Texarkana, TX-AR (Metropolitan Statistical Area)",
                               Val = "45500",
                           },
                           new GeoFips
                           {
                               Description = "5 The Villages, FL (Metropolitan Statistical Area)",
                               Val = "45540",
                           },
                           new GeoFips
                           {
                               Description = "5 Toledo, OH (Metropolitan Statistical Area)",
                               Val = "45780",
                           },
                           new GeoFips
                           {
                               Description = "5 Topeka, KS (Metropolitan Statistical Area)",
                               Val = "45820",
                           },
                           new GeoFips
                           {
                               Description = "5 Trenton, NJ (Metropolitan Statistical Area)",
                               Val = "45940",
                           },
                           new GeoFips
                           {
                               Description = "3 South Dakota",
                               Val = "46000",
                           },
                           new GeoFips
                           {
                               Description = "4 Aurora, SD ",
                               Val = "46003",
                           },
                           new GeoFips
                           {
                               Description = "4 Beadle, SD ",
                               Val = "46005",
                           },
                           new GeoFips
                           {
                               Description = "4 Bennett, SD ",
                               Val = "46007",
                           },
                           new GeoFips
                           {
                               Description = "4 Bon Homme, SD ",
                               Val = "46009",
                           },
                           new GeoFips
                           {
                               Description = "4 Brookings, SD ",
                               Val = "46011",
                           },
                           new GeoFips
                           {
                               Description = "4 Brown, SD ",
                               Val = "46013",
                           },
                           new GeoFips
                           {
                               Description = "4 Brule, SD ",
                               Val = "46015",
                           },
                           new GeoFips
                           {
                               Description = "4 Buffalo, SD ",
                               Val = "46017",
                           },
                           new GeoFips
                           {
                               Description = "4 Butte, SD ",
                               Val = "46019",
                           },
                           new GeoFips
                           {
                               Description = "4 Campbell, SD ",
                               Val = "46021",
                           },
                           new GeoFips
                           {
                               Description = "4 Charles Mix, SD ",
                               Val = "46023",
                           },
                           new GeoFips
                           {
                               Description = "4 Clark, SD ",
                               Val = "46025",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, SD ",
                               Val = "46027",
                           },
                           new GeoFips
                           {
                               Description = "4 Codington, SD ",
                               Val = "46029",
                           },
                           new GeoFips
                           {
                               Description = "4 Corson, SD ",
                               Val = "46031",
                           },
                           new GeoFips
                           {
                               Description = "4 Custer, SD ",
                               Val = "46033",
                           },
                           new GeoFips
                           {
                               Description = "4 Davison, SD ",
                               Val = "46035",
                           },
                           new GeoFips
                           {
                               Description = "4 Day, SD ",
                               Val = "46037",
                           },
                           new GeoFips
                           {
                               Description = "4 Deuel, SD ",
                               Val = "46039",
                           },
                           new GeoFips
                           {
                               Description = "4 Dewey, SD ",
                               Val = "46041",
                           },
                           new GeoFips
                           {
                               Description = "4 Douglas, SD ",
                               Val = "46043",
                           },
                           new GeoFips
                           {
                               Description = "4 Edmunds, SD ",
                               Val = "46045",
                           },
                           new GeoFips
                           {
                               Description = "4 Fall River, SD ",
                               Val = "46047",
                           },
                           new GeoFips
                           {
                               Description = "4 Faulk, SD ",
                               Val = "46049",
                           },
                           new GeoFips
                           {
                               Description = "4 Grant, SD ",
                               Val = "46051",
                           },
                           new GeoFips
                           {
                               Description = "4 Gregory, SD ",
                               Val = "46053",
                           },
                           new GeoFips
                           {
                               Description = "4 Haakon, SD ",
                               Val = "46055",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamlin, SD ",
                               Val = "46057",
                           },
                           new GeoFips
                           {
                               Description = "4 Hand, SD ",
                               Val = "46059",
                           },
                           new GeoFips
                           {
                               Description = "5 Tucson, AZ (Metropolitan Statistical Area)",
                               Val = "46060",
                           },
                           new GeoFips
                           {
                               Description = "4 Hanson, SD ",
                               Val = "46061",
                           },
                           new GeoFips
                           {
                               Description = "4 Harding, SD ",
                               Val = "46063",
                           },
                           new GeoFips
                           {
                               Description = "4 Hughes, SD ",
                               Val = "46065",
                           },
                           new GeoFips
                           {
                               Description = "4 Hutchinson, SD ",
                               Val = "46067",
                           },
                           new GeoFips
                           {
                               Description = "4 Hyde, SD ",
                               Val = "46069",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, SD ",
                               Val = "46071",
                           },
                           new GeoFips
                           {
                               Description = "4 Jerauld, SD ",
                               Val = "46073",
                           },
                           new GeoFips
                           {
                               Description = "4 Jones, SD ",
                               Val = "46075",
                           },
                           new GeoFips
                           {
                               Description = "4 Kingsbury, SD ",
                               Val = "46077",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, SD ",
                               Val = "46079",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, SD ",
                               Val = "46081",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, SD ",
                               Val = "46083",
                           },
                           new GeoFips
                           {
                               Description = "4 Lyman, SD ",
                               Val = "46085",
                           },
                           new GeoFips
                           {
                               Description = "4 McCook, SD ",
                               Val = "46087",
                           },
                           new GeoFips
                           {
                               Description = "4 McPherson, SD ",
                               Val = "46089",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, SD ",
                               Val = "46091",
                           },
                           new GeoFips
                           {
                               Description = "4 Meade, SD ",
                               Val = "46093",
                           },
                           new GeoFips
                           {
                               Description = "4 Mellette, SD ",
                               Val = "46095",
                           },
                           new GeoFips
                           {
                               Description = "4 Miner, SD ",
                               Val = "46097",
                           },
                           new GeoFips
                           {
                               Description = "4 Minnehaha, SD ",
                               Val = "46099",
                           },
                           new GeoFips
                           {
                               Description = "4 Moody, SD ",
                               Val = "46101",
                           },
                           new GeoFips
                           {
                               Description = "4 Pennington, SD ",
                               Val = "46103",
                           },
                           new GeoFips
                           {
                               Description = "4 Perkins, SD ",
                               Val = "46105",
                           },
                           new GeoFips
                           {
                               Description = "4 Potter, SD ",
                               Val = "46107",
                           },
                           new GeoFips
                           {
                               Description = "4 Roberts, SD ",
                               Val = "46109",
                           },
                           new GeoFips
                           {
                               Description = "4 Sanborn, SD ",
                               Val = "46111",
                           },
                           new GeoFips
                           {
                               Description = "4 Shannon, SD ",
                               Val = "46113",
                           },
                           new GeoFips
                           {
                               Description = "4 Spink, SD ",
                               Val = "46115",
                           },
                           new GeoFips
                           {
                               Description = "4 Stanley, SD ",
                               Val = "46117",
                           },
                           new GeoFips
                           {
                               Description = "4 Sully, SD ",
                               Val = "46119",
                           },
                           new GeoFips
                           {
                               Description = "4 Todd, SD ",
                               Val = "46121",
                           },
                           new GeoFips
                           {
                               Description = "4 Tripp, SD ",
                               Val = "46123",
                           },
                           new GeoFips
                           {
                               Description = "4 Turner, SD ",
                               Val = "46125",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, SD ",
                               Val = "46127",
                           },
                           new GeoFips
                           {
                               Description = "4 Walworth, SD ",
                               Val = "46129",
                           },
                           new GeoFips
                           {
                               Description = "4 Yankton, SD ",
                               Val = "46135",
                           },
                           new GeoFips
                           {
                               Description = "4 Ziebach, SD ",
                               Val = "46137",
                           },
                           new GeoFips
                           {
                               Description = "5 Tulsa, OK (Metropolitan Statistical Area)",
                               Val = "46140",
                           },
                           new GeoFips
                           {
                               Description = "5 Tuscaloosa, AL (Metropolitan Statistical Area)",
                               Val = "46220",
                           },
                           new GeoFips
                           {
                               Description = "5 Tyler, TX (Metropolitan Statistical Area)",
                               Val = "46340",
                           },
                           new GeoFips
                           {
                               Description = "5 Urban Honolulu, HI (Metropolitan Statistical Area)",
                               Val = "46520",
                           },
                           new GeoFips
                           {
                               Description = "5 Utica-Rome, NY (Metropolitan Statistical Area)",
                               Val = "46540",
                           },
                           new GeoFips
                           {
                               Description = "5 Valdosta, GA (Metropolitan Statistical Area)",
                               Val = "46660",
                           },
                           new GeoFips
                           {
                               Description = "5 Vallejo-Fairfield, CA (Metropolitan Statistical Area)",
                               Val = "46700",
                           },
                           new GeoFips
                           {
                               Description = "3 Tennessee",
                               Val = "47000",
                           },
                           new GeoFips
                           {
                               Description = "4 Anderson, TN ",
                               Val = "47001",
                           },
                           new GeoFips
                           {
                               Description = "4 Bedford, TN ",
                               Val = "47003",
                           },
                           new GeoFips
                           {
                               Description = "4 Benton, TN ",
                               Val = "47005",
                           },
                           new GeoFips
                           {
                               Description = "4 Bledsoe, TN ",
                               Val = "47007",
                           },
                           new GeoFips
                           {
                               Description = "4 Blount, TN ",
                               Val = "47009",
                           },
                           new GeoFips
                           {
                               Description = "4 Bradley, TN ",
                               Val = "47011",
                           },
                           new GeoFips
                           {
                               Description = "4 Campbell, TN ",
                               Val = "47013",
                           },
                           new GeoFips
                           {
                               Description = "4 Cannon, TN ",
                               Val = "47015",
                           },
                           new GeoFips
                           {
                               Description = "4 Carroll, TN ",
                               Val = "47017",
                           },
                           new GeoFips
                           {
                               Description = "4 Carter, TN ",
                               Val = "47019",
                           },
                           new GeoFips
                           {
                               Description = "5 Victoria, TX (Metropolitan Statistical Area)",
                               Val = "47020",
                           },
                           new GeoFips
                           {
                               Description = "4 Cheatham, TN ",
                               Val = "47021",
                           },
                           new GeoFips
                           {
                               Description = "4 Chester, TN ",
                               Val = "47023",
                           },
                           new GeoFips
                           {
                               Description = "4 Claiborne, TN ",
                               Val = "47025",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, TN ",
                               Val = "47027",
                           },
                           new GeoFips
                           {
                               Description = "4 Cocke, TN ",
                               Val = "47029",
                           },
                           new GeoFips
                           {
                               Description = "4 Coffee, TN ",
                               Val = "47031",
                           },
                           new GeoFips
                           {
                               Description = "4 Crockett, TN ",
                               Val = "47033",
                           },
                           new GeoFips
                           {
                               Description = "4 Cumberland, TN ",
                               Val = "47035",
                           },
                           new GeoFips
                           {
                               Description = "4 Davidson, TN ",
                               Val = "47037",
                           },
                           new GeoFips
                           {
                               Description = "4 Decatur, TN ",
                               Val = "47039",
                           },
                           new GeoFips
                           {
                               Description = "4 DeKalb, TN ",
                               Val = "47041",
                           },
                           new GeoFips
                           {
                               Description = "4 Dickson, TN ",
                               Val = "47043",
                           },
                           new GeoFips
                           {
                               Description = "4 Dyer, TN ",
                               Val = "47045",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, TN ",
                               Val = "47047",
                           },
                           new GeoFips
                           {
                               Description = "4 Fentress, TN ",
                               Val = "47049",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, TN ",
                               Val = "47051",
                           },
                           new GeoFips
                           {
                               Description = "4 Gibson, TN ",
                               Val = "47053",
                           },
                           new GeoFips
                           {
                               Description = "4 Giles, TN ",
                               Val = "47055",
                           },
                           new GeoFips
                           {
                               Description = "4 Grainger, TN ",
                               Val = "47057",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, TN ",
                               Val = "47059",
                           },
                           new GeoFips
                           {
                               Description = "4 Grundy, TN ",
                               Val = "47061",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamblen, TN ",
                               Val = "47063",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, TN ",
                               Val = "47065",
                           },
                           new GeoFips
                           {
                               Description = "4 Hancock, TN ",
                               Val = "47067",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardeman, TN ",
                               Val = "47069",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardin, TN ",
                               Val = "47071",
                           },
                           new GeoFips
                           {
                               Description = "4 Hawkins, TN ",
                               Val = "47073",
                           },
                           new GeoFips
                           {
                               Description = "4 Haywood, TN ",
                               Val = "47075",
                           },
                           new GeoFips
                           {
                               Description = "4 Henderson, TN ",
                               Val = "47077",
                           },
                           new GeoFips
                           {
                               Description = "4 Henry, TN ",
                               Val = "47079",
                           },
                           new GeoFips
                           {
                               Description = "4 Hickman, TN ",
                               Val = "47081",
                           },
                           new GeoFips
                           {
                               Description = "4 Houston, TN ",
                               Val = "47083",
                           },
                           new GeoFips
                           {
                               Description = "4 Humphreys, TN ",
                               Val = "47085",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, TN ",
                               Val = "47087",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, TN ",
                               Val = "47089",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, TN ",
                               Val = "47091",
                           },
                           new GeoFips
                           {
                               Description = "4 Knox, TN ",
                               Val = "47093",
                           },
                           new GeoFips
                           {
                               Description = "4 Lake, TN ",
                               Val = "47095",
                           },
                           new GeoFips
                           {
                               Description = "4 Lauderdale, TN ",
                               Val = "47097",
                           },
                           new GeoFips
                           {
                               Description = "4 Lawrence, TN ",
                               Val = "47099",
                           },
                           new GeoFips
                           {
                               Description = "4 Lewis, TN ",
                               Val = "47101",
                           },
                           new GeoFips
                           {
                               Description = "4 Lincoln, TN ",
                               Val = "47103",
                           },
                           new GeoFips
                           {
                               Description = "4 Loudon, TN ",
                               Val = "47105",
                           },
                           new GeoFips
                           {
                               Description = "4 McMinn, TN ",
                               Val = "47107",
                           },
                           new GeoFips
                           {
                               Description = "4 McNairy, TN ",
                               Val = "47109",
                           },
                           new GeoFips
                           {
                               Description = "4 Macon, TN ",
                               Val = "47111",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, TN ",
                               Val = "47113",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, TN ",
                               Val = "47115",
                           },
                           new GeoFips
                           {
                               Description = "4 Marshall, TN ",
                               Val = "47117",
                           },
                           new GeoFips
                           {
                               Description = "4 Maury, TN ",
                               Val = "47119",
                           },
                           new GeoFips
                           {
                               Description = "4 Meigs, TN ",
                               Val = "47121",
                           },
                           new GeoFips
                           {
                               Description = "4 Monroe, TN ",
                               Val = "47123",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, TN ",
                               Val = "47125",
                           },
                           new GeoFips
                           {
                               Description = "4 Moore, TN ",
                               Val = "47127",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, TN ",
                               Val = "47129",
                           },
                           new GeoFips
                           {
                               Description = "4 Obion, TN ",
                               Val = "47131",
                           },
                           new GeoFips
                           {
                               Description = "4 Overton, TN ",
                               Val = "47133",
                           },
                           new GeoFips
                           {
                               Description = "4 Perry, TN ",
                               Val = "47135",
                           },
                           new GeoFips
                           {
                               Description = "4 Pickett, TN ",
                               Val = "47137",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, TN ",
                               Val = "47139",
                           },
                           new GeoFips
                           {
                               Description = "4 Putnam, TN ",
                               Val = "47141",
                           },
                           new GeoFips
                           {
                               Description = "4 Rhea, TN ",
                               Val = "47143",
                           },
                           new GeoFips
                           {
                               Description = "4 Roane, TN ",
                               Val = "47145",
                           },
                           new GeoFips
                           {
                               Description = "4 Robertson, TN ",
                               Val = "47147",
                           },
                           new GeoFips
                           {
                               Description = "4 Rutherford, TN ",
                               Val = "47149",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, TN ",
                               Val = "47151",
                           },
                           new GeoFips
                           {
                               Description = "4 Sequatchie, TN ",
                               Val = "47153",
                           },
                           new GeoFips
                           {
                               Description = "4 Sevier, TN ",
                               Val = "47155",
                           },
                           new GeoFips
                           {
                               Description = "4 Shelby, TN ",
                               Val = "47157",
                           },
                           new GeoFips
                           {
                               Description = "4 Smith, TN ",
                               Val = "47159",
                           },
                           new GeoFips
                           {
                               Description = "4 Stewart, TN ",
                               Val = "47161",
                           },
                           new GeoFips
                           {
                               Description = "4 Sullivan, TN ",
                               Val = "47163",
                           },
                           new GeoFips
                           {
                               Description = "4 Sumner, TN ",
                               Val = "47165",
                           },
                           new GeoFips
                           {
                               Description = "4 Tipton, TN ",
                               Val = "47167",
                           },
                           new GeoFips
                           {
                               Description = "4 Trousdale, TN ",
                               Val = "47169",
                           },
                           new GeoFips
                           {
                               Description = "4 Unicoi, TN ",
                               Val = "47171",
                           },
                           new GeoFips
                           {
                               Description = "4 Union, TN ",
                               Val = "47173",
                           },
                           new GeoFips
                           {
                               Description = "4 Van Buren, TN ",
                               Val = "47175",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, TN ",
                               Val = "47177",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, TN ",
                               Val = "47179",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, TN ",
                               Val = "47181",
                           },
                           new GeoFips
                           {
                               Description = "4 Weakley, TN ",
                               Val = "47183",
                           },
                           new GeoFips
                           {
                               Description = "4 White, TN ",
                               Val = "47185",
                           },
                           new GeoFips
                           {
                               Description = "4 Williamson, TN ",
                               Val = "47187",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilson, TN ",
                               Val = "47189",
                           },
                           new GeoFips
                           {
                               Description = "5 Vineland-Bridgeton, NJ (Metropolitan Statistical Area)",
                               Val = "47220",
                           },
                           new GeoFips
                           {
                               Description = "5 Virginia Beach-Norfolk-Newport News, VA-NC (Metropolitan Statistical Area)",
                               Val = "47260",
                           },
                           new GeoFips
                           {
                               Description = "5 Visalia-Porterville, CA (Metropolitan Statistical Area)",
                               Val = "47300",
                           },
                           new GeoFips
                           {
                               Description = "5 Waco, TX (Metropolitan Statistical Area)",
                               Val = "47380",
                           },
                           new GeoFips
                           {
                               Description = "5 Walla Walla, WA (Metropolitan Statistical Area)",
                               Val = "47460",
                           },
                           new GeoFips
                           {
                               Description = "5 Warner Robins, GA (Metropolitan Statistical Area)",
                               Val = "47580",
                           },
                           new GeoFips
                           {
                               Description = "5 Washington-Arlington-Alexandria, DC-VA-MD-WV (Metropolitan Statistical Area)",
                               Val = "47900",
                           },
                           new GeoFips
                           {
                               Description = "5 Waterloo-Cedar Falls, IA (Metropolitan Statistical Area)",
                               Val = "47940",
                           },
                           new GeoFips
                           {
                               Description = "3 Texas",
                               Val = "48000",
                           },
                           new GeoFips
                           {
                               Description = "4 Anderson, TX ",
                               Val = "48001",
                           },
                           new GeoFips
                           {
                               Description = "4 Andrews, TX ",
                               Val = "48003",
                           },
                           new GeoFips
                           {
                               Description = "4 Angelina, TX ",
                               Val = "48005",
                           },
                           new GeoFips
                           {
                               Description = "4 Aransas, TX ",
                               Val = "48007",
                           },
                           new GeoFips
                           {
                               Description = "4 Archer, TX ",
                               Val = "48009",
                           },
                           new GeoFips
                           {
                               Description = "4 Armstrong, TX ",
                               Val = "48011",
                           },
                           new GeoFips
                           {
                               Description = "4 Atascosa, TX ",
                               Val = "48013",
                           },
                           new GeoFips
                           {
                               Description = "4 Austin, TX ",
                               Val = "48015",
                           },
                           new GeoFips
                           {
                               Description = "4 Bailey, TX ",
                               Val = "48017",
                           },
                           new GeoFips
                           {
                               Description = "4 Bandera, TX ",
                               Val = "48019",
                           },
                           new GeoFips
                           {
                               Description = "4 Bastrop, TX ",
                               Val = "48021",
                           },
                           new GeoFips
                           {
                               Description = "4 Baylor, TX ",
                               Val = "48023",
                           },
                           new GeoFips
                           {
                               Description = "4 Bee, TX ",
                               Val = "48025",
                           },
                           new GeoFips
                           {
                               Description = "4 Bell, TX ",
                               Val = "48027",
                           },
                           new GeoFips
                           {
                               Description = "4 Bexar, TX ",
                               Val = "48029",
                           },
                           new GeoFips
                           {
                               Description = "4 Blanco, TX ",
                               Val = "48031",
                           },
                           new GeoFips
                           {
                               Description = "4 Borden, TX ",
                               Val = "48033",
                           },
                           new GeoFips
                           {
                               Description = "4 Bosque, TX ",
                               Val = "48035",
                           },
                           new GeoFips
                           {
                               Description = "4 Bowie, TX ",
                               Val = "48037",
                           },
                           new GeoFips
                           {
                               Description = "4 Brazoria, TX ",
                               Val = "48039",
                           },
                           new GeoFips
                           {
                               Description = "4 Brazos, TX ",
                               Val = "48041",
                           },
                           new GeoFips
                           {
                               Description = "4 Brewster, TX ",
                               Val = "48043",
                           },
                           new GeoFips
                           {
                               Description = "4 Briscoe, TX ",
                               Val = "48045",
                           },
                           new GeoFips
                           {
                               Description = "4 Brooks, TX ",
                               Val = "48047",
                           },
                           new GeoFips
                           {
                               Description = "4 Brown, TX ",
                               Val = "48049",
                           },
                           new GeoFips
                           {
                               Description = "4 Burleson, TX ",
                               Val = "48051",
                           },
                           new GeoFips
                           {
                               Description = "4 Burnet, TX ",
                               Val = "48053",
                           },
                           new GeoFips
                           {
                               Description = "4 Caldwell, TX ",
                               Val = "48055",
                           },
                           new GeoFips
                           {
                               Description = "4 Calhoun, TX ",
                               Val = "48057",
                           },
                           new GeoFips
                           {
                               Description = "4 Callahan, TX ",
                               Val = "48059",
                           },
                           new GeoFips
                           {
                               Description = "5 Watertown-Fort Drum, NY (Metropolitan Statistical Area)",
                               Val = "48060",
                           },
                           new GeoFips
                           {
                               Description = "4 Cameron, TX ",
                               Val = "48061",
                           },
                           new GeoFips
                           {
                               Description = "4 Camp, TX ",
                               Val = "48063",
                           },
                           new GeoFips
                           {
                               Description = "4 Carson, TX ",
                               Val = "48065",
                           },
                           new GeoFips
                           {
                               Description = "4 Cass, TX ",
                               Val = "48067",
                           },
                           new GeoFips
                           {
                               Description = "4 Castro, TX ",
                               Val = "48069",
                           },
                           new GeoFips
                           {
                               Description = "4 Chambers, TX ",
                               Val = "48071",
                           },
                           new GeoFips
                           {
                               Description = "4 Cherokee, TX ",
                               Val = "48073",
                           },
                           new GeoFips
                           {
                               Description = "4 Childress, TX ",
                               Val = "48075",
                           },
                           new GeoFips
                           {
                               Description = "4 Clay, TX ",
                               Val = "48077",
                           },
                           new GeoFips
                           {
                               Description = "4 Cochran, TX ",
                               Val = "48079",
                           },
                           new GeoFips
                           {
                               Description = "4 Coke, TX ",
                               Val = "48081",
                           },
                           new GeoFips
                           {
                               Description = "4 Coleman, TX ",
                               Val = "48083",
                           },
                           new GeoFips
                           {
                               Description = "4 Collin, TX ",
                               Val = "48085",
                           },
                           new GeoFips
                           {
                               Description = "4 Collingsworth, TX ",
                               Val = "48087",
                           },
                           new GeoFips
                           {
                               Description = "4 Colorado, TX ",
                               Val = "48089",
                           },
                           new GeoFips
                           {
                               Description = "4 Comal, TX ",
                               Val = "48091",
                           },
                           new GeoFips
                           {
                               Description = "4 Comanche, TX ",
                               Val = "48093",
                           },
                           new GeoFips
                           {
                               Description = "4 Concho, TX ",
                               Val = "48095",
                           },
                           new GeoFips
                           {
                               Description = "4 Cooke, TX ",
                               Val = "48097",
                           },
                           new GeoFips
                           {
                               Description = "4 Coryell, TX ",
                               Val = "48099",
                           },
                           new GeoFips
                           {
                               Description = "4 Cottle, TX ",
                               Val = "48101",
                           },
                           new GeoFips
                           {
                               Description = "4 Crane, TX ",
                               Val = "48103",
                           },
                           new GeoFips
                           {
                               Description = "4 Crockett, TX ",
                               Val = "48105",
                           },
                           new GeoFips
                           {
                               Description = "4 Crosby, TX ",
                               Val = "48107",
                           },
                           new GeoFips
                           {
                               Description = "4 Culberson, TX ",
                               Val = "48109",
                           },
                           new GeoFips
                           {
                               Description = "4 Dallam, TX ",
                               Val = "48111",
                           },
                           new GeoFips
                           {
                               Description = "4 Dallas, TX ",
                               Val = "48113",
                           },
                           new GeoFips
                           {
                               Description = "4 Dawson, TX ",
                               Val = "48115",
                           },
                           new GeoFips
                           {
                               Description = "4 Deaf Smith, TX ",
                               Val = "48117",
                           },
                           new GeoFips
                           {
                               Description = "4 Delta, TX ",
                               Val = "48119",
                           },
                           new GeoFips
                           {
                               Description = "4 Denton, TX ",
                               Val = "48121",
                           },
                           new GeoFips
                           {
                               Description = "4 DeWitt, TX ",
                               Val = "48123",
                           },
                           new GeoFips
                           {
                               Description = "4 Dickens, TX ",
                               Val = "48125",
                           },
                           new GeoFips
                           {
                               Description = "4 Dimmit, TX ",
                               Val = "48127",
                           },
                           new GeoFips
                           {
                               Description = "4 Donley, TX ",
                               Val = "48129",
                           },
                           new GeoFips
                           {
                               Description = "4 Duval, TX ",
                               Val = "48131",
                           },
                           new GeoFips
                           {
                               Description = "4 Eastland, TX ",
                               Val = "48133",
                           },
                           new GeoFips
                           {
                               Description = "4 Ector, TX ",
                               Val = "48135",
                           },
                           new GeoFips
                           {
                               Description = "4 Edwards, TX ",
                               Val = "48137",
                           },
                           new GeoFips
                           {
                               Description = "4 Ellis, TX ",
                               Val = "48139",
                           },
                           new GeoFips
                           {
                               Description = "5 Wausau, WI (Metropolitan Statistical Area)",
                               Val = "48140",
                           },
                           new GeoFips
                           {
                               Description = "4 El Paso, TX ",
                               Val = "48141",
                           },
                           new GeoFips
                           {
                               Description = "4 Erath, TX ",
                               Val = "48143",
                           },
                           new GeoFips
                           {
                               Description = "4 Falls, TX ",
                               Val = "48145",
                           },
                           new GeoFips
                           {
                               Description = "4 Fannin, TX ",
                               Val = "48147",
                           },
                           new GeoFips
                           {
                               Description = "4 Fayette, TX ",
                               Val = "48149",
                           },
                           new GeoFips
                           {
                               Description = "4 Fisher, TX ",
                               Val = "48151",
                           },
                           new GeoFips
                           {
                               Description = "4 Floyd, TX ",
                               Val = "48153",
                           },
                           new GeoFips
                           {
                               Description = "4 Foard, TX ",
                               Val = "48155",
                           },
                           new GeoFips
                           {
                               Description = "4 Fort Bend, TX ",
                               Val = "48157",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, TX ",
                               Val = "48159",
                           },
                           new GeoFips
                           {
                               Description = "4 Freestone, TX ",
                               Val = "48161",
                           },
                           new GeoFips
                           {
                               Description = "4 Frio, TX ",
                               Val = "48163",
                           },
                           new GeoFips
                           {
                               Description = "4 Gaines, TX ",
                               Val = "48165",
                           },
                           new GeoFips
                           {
                               Description = "4 Galveston, TX ",
                               Val = "48167",
                           },
                           new GeoFips
                           {
                               Description = "4 Garza, TX ",
                               Val = "48169",
                           },
                           new GeoFips
                           {
                               Description = "4 Gillespie, TX ",
                               Val = "48171",
                           },
                           new GeoFips
                           {
                               Description = "4 Glasscock, TX ",
                               Val = "48173",
                           },
                           new GeoFips
                           {
                               Description = "4 Goliad, TX ",
                               Val = "48175",
                           },
                           new GeoFips
                           {
                               Description = "4 Gonzales, TX ",
                               Val = "48177",
                           },
                           new GeoFips
                           {
                               Description = "4 Gray, TX ",
                               Val = "48179",
                           },
                           new GeoFips
                           {
                               Description = "4 Grayson, TX ",
                               Val = "48181",
                           },
                           new GeoFips
                           {
                               Description = "4 Gregg, TX ",
                               Val = "48183",
                           },
                           new GeoFips
                           {
                               Description = "4 Grimes, TX ",
                               Val = "48185",
                           },
                           new GeoFips
                           {
                               Description = "4 Guadalupe, TX ",
                               Val = "48187",
                           },
                           new GeoFips
                           {
                               Description = "4 Hale, TX ",
                               Val = "48189",
                           },
                           new GeoFips
                           {
                               Description = "4 Hall, TX ",
                               Val = "48191",
                           },
                           new GeoFips
                           {
                               Description = "4 Hamilton, TX ",
                               Val = "48193",
                           },
                           new GeoFips
                           {
                               Description = "4 Hansford, TX ",
                               Val = "48195",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardeman, TX ",
                               Val = "48197",
                           },
                           new GeoFips
                           {
                               Description = "4 Hardin, TX ",
                               Val = "48199",
                           },
                           new GeoFips
                           {
                               Description = "4 Harris, TX ",
                               Val = "48201",
                           },
                           new GeoFips
                           {
                               Description = "4 Harrison, TX ",
                               Val = "48203",
                           },
                           new GeoFips
                           {
                               Description = "4 Hartley, TX ",
                               Val = "48205",
                           },
                           new GeoFips
                           {
                               Description = "4 Haskell, TX ",
                               Val = "48207",
                           },
                           new GeoFips
                           {
                               Description = "4 Hays, TX ",
                               Val = "48209",
                           },
                           new GeoFips
                           {
                               Description = "4 Hemphill, TX ",
                               Val = "48211",
                           },
                           new GeoFips
                           {
                               Description = "4 Henderson, TX ",
                               Val = "48213",
                           },
                           new GeoFips
                           {
                               Description = "4 Hidalgo, TX ",
                               Val = "48215",
                           },
                           new GeoFips
                           {
                               Description = "4 Hill, TX ",
                               Val = "48217",
                           },
                           new GeoFips
                           {
                               Description = "4 Hockley, TX ",
                               Val = "48219",
                           },
                           new GeoFips
                           {
                               Description = "4 Hood, TX ",
                               Val = "48221",
                           },
                           new GeoFips
                           {
                               Description = "4 Hopkins, TX ",
                               Val = "48223",
                           },
                           new GeoFips
                           {
                               Description = "4 Houston, TX ",
                               Val = "48225",
                           },
                           new GeoFips
                           {
                               Description = "4 Howard, TX ",
                               Val = "48227",
                           },
                           new GeoFips
                           {
                               Description = "4 Hudspeth, TX ",
                               Val = "48229",
                           },
                           new GeoFips
                           {
                               Description = "4 Hunt, TX ",
                               Val = "48231",
                           },
                           new GeoFips
                           {
                               Description = "4 Hutchinson, TX ",
                               Val = "48233",
                           },
                           new GeoFips
                           {
                               Description = "4 Irion, TX ",
                               Val = "48235",
                           },
                           new GeoFips
                           {
                               Description = "4 Jack, TX ",
                               Val = "48237",
                           },
                           new GeoFips
                           {
                               Description = "4 Jackson, TX ",
                               Val = "48239",
                           },
                           new GeoFips
                           {
                               Description = "4 Jasper, TX ",
                               Val = "48241",
                           },
                           new GeoFips
                           {
                               Description = "4 Jeff Davis, TX ",
                               Val = "48243",
                           },
                           new GeoFips
                           {
                               Description = "4 Jefferson, TX ",
                               Val = "48245",
                           },
                           new GeoFips
                           {
                               Description = "4 Jim Hogg, TX ",
                               Val = "48247",
                           },
                           new GeoFips
                           {
                               Description = "4 Jim Wells, TX ",
                               Val = "48249",
                           },
                           new GeoFips
                           {
                               Description = "4 Johnson, TX ",
                               Val = "48251",
                           },
                           new GeoFips
                           {
                               Description = "4 Jones, TX ",
                               Val = "48253",
                           },
                           new GeoFips
                           {
                               Description = "4 Karnes, TX ",
                               Val = "48255",
                           },
                           new GeoFips
                           {
                               Description = "4 Kaufman, TX ",
                               Val = "48257",
                           },
                           new GeoFips
                           {
                               Description = "4 Kendall, TX ",
                               Val = "48259",
                           },
                           new GeoFips
                           {
                               Description = "5 Weirton-Steubenville, WV-OH (Metropolitan Statistical Area)",
                               Val = "48260",
                           },
                           new GeoFips
                           {
                               Description = "4 Kenedy, TX ",
                               Val = "48261",
                           },
                           new GeoFips
                           {
                               Description = "4 Kent, TX ",
                               Val = "48263",
                           },
                           new GeoFips
                           {
                               Description = "4 Kerr, TX ",
                               Val = "48265",
                           },
                           new GeoFips
                           {
                               Description = "4 Kimble, TX ",
                               Val = "48267",
                           },
                           new GeoFips
                           {
                               Description = "4 King, TX ",
                               Val = "48269",
                           },
                           new GeoFips
                           {
                               Description = "4 Kinney, TX ",
                               Val = "48271",
                           },
                           new GeoFips
                           {
                               Description = "4 Kleberg, TX ",
                               Val = "48273",
                           },
                           new GeoFips
                           {
                               Description = "4 Knox, TX ",
                               Val = "48275",
                           },
                           new GeoFips
                           {
                               Description = "4 Lamar, TX ",
                               Val = "48277",
                           },
                           new GeoFips
                           {
                               Description = "4 Lamb, TX ",
                               Val = "48279",
                           },
                           new GeoFips
                           {
                               Description = "4 Lampasas, TX ",
                               Val = "48281",
                           },
                           new GeoFips
                           {
                               Description = "4 La Salle, TX ",
                               Val = "48283",
                           },
                           new GeoFips
                           {
                               Description = "4 Lavaca, TX ",
                               Val = "48285",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, TX ",
                               Val = "48287",
                           },
                           new GeoFips
                           {
                               Description = "4 Leon, TX ",
                               Val = "48289",
                           },
                           new GeoFips
                           {
                               Description = "4 Liberty, TX ",
                               Val = "48291",
                           },
                           new GeoFips
                           {
                               Description = "4 Limestone, TX ",
                               Val = "48293",
                           },
                           new GeoFips
                           {
                               Description = "4 Lipscomb, TX ",
                               Val = "48295",
                           },
                           new GeoFips
                           {
                               Description = "4 Live Oak, TX ",
                               Val = "48297",
                           },
                           new GeoFips
                           {
                               Description = "4 Llano, TX ",
                               Val = "48299",
                           },
                           new GeoFips
                           {
                               Description = "5 Wenatchee, WA (Metropolitan Statistical Area)",
                               Val = "48300",
                           },
                           new GeoFips
                           {
                               Description = "4 Loving, TX ",
                               Val = "48301",
                           },
                           new GeoFips
                           {
                               Description = "4 Lubbock, TX ",
                               Val = "48303",
                           },
                           new GeoFips
                           {
                               Description = "4 Lynn, TX ",
                               Val = "48305",
                           },
                           new GeoFips
                           {
                               Description = "4 McCulloch, TX ",
                               Val = "48307",
                           },
                           new GeoFips
                           {
                               Description = "4 McLennan, TX ",
                               Val = "48309",
                           },
                           new GeoFips
                           {
                               Description = "4 McMullen, TX ",
                               Val = "48311",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, TX ",
                               Val = "48313",
                           },
                           new GeoFips
                           {
                               Description = "4 Marion, TX ",
                               Val = "48315",
                           },
                           new GeoFips
                           {
                               Description = "4 Martin, TX ",
                               Val = "48317",
                           },
                           new GeoFips
                           {
                               Description = "4 Mason, TX ",
                               Val = "48319",
                           },
                           new GeoFips
                           {
                               Description = "4 Matagorda, TX ",
                               Val = "48321",
                           },
                           new GeoFips
                           {
                               Description = "4 Maverick, TX ",
                               Val = "48323",
                           },
                           new GeoFips
                           {
                               Description = "4 Medina, TX ",
                               Val = "48325",
                           },
                           new GeoFips
                           {
                               Description = "4 Menard, TX ",
                               Val = "48327",
                           },
                           new GeoFips
                           {
                               Description = "4 Midland, TX ",
                               Val = "48329",
                           },
                           new GeoFips
                           {
                               Description = "4 Milam, TX ",
                               Val = "48331",
                           },
                           new GeoFips
                           {
                               Description = "4 Mills, TX ",
                               Val = "48333",
                           },
                           new GeoFips
                           {
                               Description = "4 Mitchell, TX ",
                               Val = "48335",
                           },
                           new GeoFips
                           {
                               Description = "4 Montague, TX ",
                               Val = "48337",
                           },
                           new GeoFips
                           {
                               Description = "4 Montgomery, TX ",
                               Val = "48339",
                           },
                           new GeoFips
                           {
                               Description = "4 Moore, TX ",
                               Val = "48341",
                           },
                           new GeoFips
                           {
                               Description = "4 Morris, TX ",
                               Val = "48343",
                           },
                           new GeoFips
                           {
                               Description = "4 Motley, TX ",
                               Val = "48345",
                           },
                           new GeoFips
                           {
                               Description = "4 Nacogdoches, TX ",
                               Val = "48347",
                           },
                           new GeoFips
                           {
                               Description = "4 Navarro, TX ",
                               Val = "48349",
                           },
                           new GeoFips
                           {
                               Description = "4 Newton, TX ",
                               Val = "48351",
                           },
                           new GeoFips
                           {
                               Description = "4 Nolan, TX ",
                               Val = "48353",
                           },
                           new GeoFips
                           {
                               Description = "4 Nueces, TX ",
                               Val = "48355",
                           },
                           new GeoFips
                           {
                               Description = "4 Ochiltree, TX ",
                               Val = "48357",
                           },
                           new GeoFips
                           {
                               Description = "4 Oldham, TX ",
                               Val = "48359",
                           },
                           new GeoFips
                           {
                               Description = "4 Orange, TX ",
                               Val = "48361",
                           },
                           new GeoFips
                           {
                               Description = "4 Palo Pinto, TX ",
                               Val = "48363",
                           },
                           new GeoFips
                           {
                               Description = "4 Panola, TX ",
                               Val = "48365",
                           },
                           new GeoFips
                           {
                               Description = "4 Parker, TX ",
                               Val = "48367",
                           },
                           new GeoFips
                           {
                               Description = "4 Parmer, TX ",
                               Val = "48369",
                           },
                           new GeoFips
                           {
                               Description = "4 Pecos, TX ",
                               Val = "48371",
                           },
                           new GeoFips
                           {
                               Description = "4 Polk, TX ",
                               Val = "48373",
                           },
                           new GeoFips
                           {
                               Description = "4 Potter, TX ",
                               Val = "48375",
                           },
                           new GeoFips
                           {
                               Description = "4 Presidio, TX ",
                               Val = "48377",
                           },
                           new GeoFips
                           {
                               Description = "4 Rains, TX ",
                               Val = "48379",
                           },
                           new GeoFips
                           {
                               Description = "4 Randall, TX ",
                               Val = "48381",
                           },
                           new GeoFips
                           {
                               Description = "4 Reagan, TX ",
                               Val = "48383",
                           },
                           new GeoFips
                           {
                               Description = "4 Real, TX ",
                               Val = "48385",
                           },
                           new GeoFips
                           {
                               Description = "4 Red River, TX ",
                               Val = "48387",
                           },
                           new GeoFips
                           {
                               Description = "4 Reeves, TX ",
                               Val = "48389",
                           },
                           new GeoFips
                           {
                               Description = "4 Refugio, TX ",
                               Val = "48391",
                           },
                           new GeoFips
                           {
                               Description = "4 Roberts, TX ",
                               Val = "48393",
                           },
                           new GeoFips
                           {
                               Description = "4 Robertson, TX ",
                               Val = "48395",
                           },
                           new GeoFips
                           {
                               Description = "4 Rockwall, TX ",
                               Val = "48397",
                           },
                           new GeoFips
                           {
                               Description = "4 Runnels, TX ",
                               Val = "48399",
                           },
                           new GeoFips
                           {
                               Description = "4 Rusk, TX ",
                               Val = "48401",
                           },
                           new GeoFips
                           {
                               Description = "4 Sabine, TX ",
                               Val = "48403",
                           },
                           new GeoFips
                           {
                               Description = "4 San Augustine, TX ",
                               Val = "48405",
                           },
                           new GeoFips
                           {
                               Description = "4 San Jacinto, TX ",
                               Val = "48407",
                           },
                           new GeoFips
                           {
                               Description = "4 San Patricio, TX ",
                               Val = "48409",
                           },
                           new GeoFips
                           {
                               Description = "4 San Saba, TX ",
                               Val = "48411",
                           },
                           new GeoFips
                           {
                               Description = "4 Schleicher, TX ",
                               Val = "48413",
                           },
                           new GeoFips
                           {
                               Description = "4 Scurry, TX ",
                               Val = "48415",
                           },
                           new GeoFips
                           {
                               Description = "4 Shackelford, TX ",
                               Val = "48417",
                           },
                           new GeoFips
                           {
                               Description = "4 Shelby, TX ",
                               Val = "48419",
                           },
                           new GeoFips
                           {
                               Description = "4 Sherman, TX ",
                               Val = "48421",
                           },
                           new GeoFips
                           {
                               Description = "4 Smith, TX ",
                               Val = "48423",
                           },
                           new GeoFips
                           {
                               Description = "4 Somervell, TX ",
                               Val = "48425",
                           },
                           new GeoFips
                           {
                               Description = "4 Starr, TX ",
                               Val = "48427",
                           },
                           new GeoFips
                           {
                               Description = "4 Stephens, TX ",
                               Val = "48429",
                           },
                           new GeoFips
                           {
                               Description = "4 Sterling, TX ",
                               Val = "48431",
                           },
                           new GeoFips
                           {
                               Description = "4 Stonewall, TX ",
                               Val = "48433",
                           },
                           new GeoFips
                           {
                               Description = "4 Sutton, TX ",
                               Val = "48435",
                           },
                           new GeoFips
                           {
                               Description = "4 Swisher, TX ",
                               Val = "48437",
                           },
                           new GeoFips
                           {
                               Description = "4 Tarrant, TX ",
                               Val = "48439",
                           },
                           new GeoFips
                           {
                               Description = "4 Taylor, TX ",
                               Val = "48441",
                           },
                           new GeoFips
                           {
                               Description = "4 Terrell, TX ",
                               Val = "48443",
                           },
                           new GeoFips
                           {
                               Description = "4 Terry, TX ",
                               Val = "48445",
                           },
                           new GeoFips
                           {
                               Description = "4 Throckmorton, TX ",
                               Val = "48447",
                           },
                           new GeoFips
                           {
                               Description = "4 Titus, TX ",
                               Val = "48449",
                           },
                           new GeoFips
                           {
                               Description = "4 Tom Green, TX ",
                               Val = "48451",
                           },
                           new GeoFips
                           {
                               Description = "4 Travis, TX ",
                               Val = "48453",
                           },
                           new GeoFips
                           {
                               Description = "4 Trinity, TX ",
                               Val = "48455",
                           },
                           new GeoFips
                           {
                               Description = "4 Tyler, TX ",
                               Val = "48457",
                           },
                           new GeoFips
                           {
                               Description = "4 Upshur, TX ",
                               Val = "48459",
                           },
                           new GeoFips
                           {
                               Description = "4 Upton, TX ",
                               Val = "48461",
                           },
                           new GeoFips
                           {
                               Description = "4 Uvalde, TX ",
                               Val = "48463",
                           },
                           new GeoFips
                           {
                               Description = "4 Val Verde, TX ",
                               Val = "48465",
                           },
                           new GeoFips
                           {
                               Description = "4 Van Zandt, TX ",
                               Val = "48467",
                           },
                           new GeoFips
                           {
                               Description = "4 Victoria, TX ",
                               Val = "48469",
                           },
                           new GeoFips
                           {
                               Description = "4 Walker, TX ",
                               Val = "48471",
                           },
                           new GeoFips
                           {
                               Description = "4 Waller, TX ",
                               Val = "48473",
                           },
                           new GeoFips
                           {
                               Description = "4 Ward, TX ",
                               Val = "48475",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, TX ",
                               Val = "48477",
                           },
                           new GeoFips
                           {
                               Description = "4 Webb, TX ",
                               Val = "48479",
                           },
                           new GeoFips
                           {
                               Description = "4 Wharton, TX ",
                               Val = "48481",
                           },
                           new GeoFips
                           {
                               Description = "4 Wheeler, TX ",
                               Val = "48483",
                           },
                           new GeoFips
                           {
                               Description = "4 Wichita, TX ",
                               Val = "48485",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilbarger, TX ",
                               Val = "48487",
                           },
                           new GeoFips
                           {
                               Description = "4 Willacy, TX ",
                               Val = "48489",
                           },
                           new GeoFips
                           {
                               Description = "4 Williamson, TX ",
                               Val = "48491",
                           },
                           new GeoFips
                           {
                               Description = "4 Wilson, TX ",
                               Val = "48493",
                           },
                           new GeoFips
                           {
                               Description = "4 Winkler, TX ",
                               Val = "48495",
                           },
                           new GeoFips
                           {
                               Description = "4 Wise, TX ",
                               Val = "48497",
                           },
                           new GeoFips
                           {
                               Description = "4 Wood, TX ",
                               Val = "48499",
                           },
                           new GeoFips
                           {
                               Description = "4 Yoakum, TX ",
                               Val = "48501",
                           },
                           new GeoFips
                           {
                               Description = "4 Young, TX ",
                               Val = "48503",
                           },
                           new GeoFips
                           {
                               Description = "4 Zapata, TX ",
                               Val = "48505",
                           },
                           new GeoFips
                           {
                               Description = "4 Zavala, TX ",
                               Val = "48507",
                           },
                           new GeoFips
                           {
                               Description = "5 Wheeling, WV-OH (Metropolitan Statistical Area)",
                               Val = "48540",
                           },
                           new GeoFips
                           {
                               Description = "5 Wichita, KS (Metropolitan Statistical Area)",
                               Val = "48620",
                           },
                           new GeoFips
                           {
                               Description = "5 Wichita Falls, TX (Metropolitan Statistical Area)",
                               Val = "48660",
                           },
                           new GeoFips
                           {
                               Description = "5 Williamsport, PA (Metropolitan Statistical Area)",
                               Val = "48700",
                           },
                           new GeoFips
                           {
                               Description = "5 Wilmington, NC (Metropolitan Statistical Area)",
                               Val = "48900",
                           },
                           new GeoFips
                           {
                               Description = "3 Utah",
                               Val = "49000",
                           },
                           new GeoFips
                           {
                               Description = "4 Beaver, UT ",
                               Val = "49001",
                           },
                           new GeoFips
                           {
                               Description = "4 Box Elder, UT ",
                               Val = "49003",
                           },
                           new GeoFips
                           {
                               Description = "4 Cache, UT ",
                               Val = "49005",
                           },
                           new GeoFips
                           {
                               Description = "4 Carbon, UT ",
                               Val = "49007",
                           },
                           new GeoFips
                           {
                               Description = "4 Daggett, UT ",
                               Val = "49009",
                           },
                           new GeoFips
                           {
                               Description = "4 Davis, UT ",
                               Val = "49011",
                           },
                           new GeoFips
                           {
                               Description = "4 Duchesne, UT ",
                               Val = "49013",
                           },
                           new GeoFips
                           {
                               Description = "4 Emery, UT ",
                               Val = "49015",
                           },
                           new GeoFips
                           {
                               Description = "4 Garfield, UT ",
                               Val = "49017",
                           },
                           new GeoFips
                           {
                               Description = "4 Grand, UT ",
                               Val = "49019",
                           },
                           new GeoFips
                           {
                               Description = "5 Winchester, VA-WV (Metropolitan Statistical Area)",
                               Val = "49020",
                           },
                           new GeoFips
                           {
                               Description = "4 Iron, UT ",
                               Val = "49021",
                           },
                           new GeoFips
                           {
                               Description = "4 Juab, UT ",
                               Val = "49023",
                           },
                           new GeoFips
                           {
                               Description = "4 Kane, UT ",
                               Val = "49025",
                           },
                           new GeoFips
                           {
                               Description = "4 Millard, UT ",
                               Val = "49027",
                           },
                           new GeoFips
                           {
                               Description = "4 Morgan, UT ",
                               Val = "49029",
                           },
                           new GeoFips
                           {
                               Description = "4 Piute, UT ",
                               Val = "49031",
                           },
                           new GeoFips
                           {
                               Description = "4 Rich, UT ",
                               Val = "49033",
                           },
                           new GeoFips
                           {
                               Description = "4 Salt Lake, UT ",
                               Val = "49035",
                           },
                           new GeoFips
                           {
                               Description = "4 San Juan, UT ",
                               Val = "49037",
                           },
                           new GeoFips
                           {
                               Description = "4 Sanpete, UT ",
                               Val = "49039",
                           },
                           new GeoFips
                           {
                               Description = "4 Sevier, UT ",
                               Val = "49041",
                           },
                           new GeoFips
                           {
                               Description = "4 Summit, UT ",
                               Val = "49043",
                           },
                           new GeoFips
                           {
                               Description = "4 Tooele, UT ",
                               Val = "49045",
                           },
                           new GeoFips
                           {
                               Description = "4 Uintah, UT ",
                               Val = "49047",
                           },
                           new GeoFips
                           {
                               Description = "4 Utah, UT ",
                               Val = "49049",
                           },
                           new GeoFips
                           {
                               Description = "4 Wasatch, UT ",
                               Val = "49051",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, UT ",
                               Val = "49053",
                           },
                           new GeoFips
                           {
                               Description = "4 Wayne, UT ",
                               Val = "49055",
                           },
                           new GeoFips
                           {
                               Description = "4 Weber, UT ",
                               Val = "49057",
                           },
                           new GeoFips
                           {
                               Description = "5 Winston-Salem, NC (Metropolitan Statistical Area)",
                               Val = "49180",
                           },
                           new GeoFips
                           {
                               Description = "5 Worcester, MA-CT (Metropolitan Statistical Area)",
                               Val = "49340",
                           },
                           new GeoFips
                           {
                               Description = "5 Yakima, WA (Metropolitan Statistical Area)",
                               Val = "49420",
                           },
                           new GeoFips
                           {
                               Description = "5 York-Hanover, PA (Metropolitan Statistical Area)",
                               Val = "49620",
                           },
                           new GeoFips
                           {
                               Description = "5 Youngstown-Warren-Boardman, OH-PA (Metropolitan Statistical Area)",
                               Val = "49660",
                           },
                           new GeoFips
                           {
                               Description = "5 Yuba City, CA (Metropolitan Statistical Area)",
                               Val = "49700",
                           },
                           new GeoFips
                           {
                               Description = "5 Yuma, AZ (Metropolitan Statistical Area)",
                               Val = "49740",
                           },
                           new GeoFips
                           {
                               Description = "3 Vermont",
                               Val = "50000",
                           },
                           new GeoFips
                           {
                               Description = "4 Addison, VT ",
                               Val = "50001",
                           },
                           new GeoFips
                           {
                               Description = "4 Bennington, VT ",
                               Val = "50003",
                           },
                           new GeoFips
                           {
                               Description = "4 Caledonia, VT ",
                               Val = "50005",
                           },
                           new GeoFips
                           {
                               Description = "4 Chittenden, VT ",
                               Val = "50007",
                           },
                           new GeoFips
                           {
                               Description = "4 Essex, VT ",
                               Val = "50009",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, VT ",
                               Val = "50011",
                           },
                           new GeoFips
                           {
                               Description = "4 Grand Isle, VT ",
                               Val = "50013",
                           },
                           new GeoFips
                           {
                               Description = "4 Lamoille, VT ",
                               Val = "50015",
                           },
                           new GeoFips
                           {
                               Description = "4 Orange, VT ",
                               Val = "50017",
                           },
                           new GeoFips
                           {
                               Description = "4 Orleans, VT ",
                               Val = "50019",
                           },
                           new GeoFips
                           {
                               Description = "4 Rutland, VT ",
                               Val = "50021",
                           },
                           new GeoFips
                           {
                               Description = "4 Washington, VT ",
                               Val = "50023",
                           },
                           new GeoFips
                           {
                               Description = "4 Windham, VT ",
                               Val = "50025",
                           },
                           new GeoFips
                           {
                               Description = "4 Windsor, VT ",
                               Val = "50027",
                           },
                           new GeoFips
                           {
                               Description = "3 Virginia",
                               Val = "51000",
                           },
                           new GeoFips
                           {
                               Description = "4 Accomack, VA ",
                               Val = "51001",
                           },
                           new GeoFips
                           {
                               Description = "4 Amelia, VA ",
                               Val = "51007",
                           },
                           new GeoFips
                           {
                               Description = "4 Amherst, VA ",
                               Val = "51009",
                           },
                           new GeoFips
                           {
                               Description = "4 Appomattox, VA ",
                               Val = "51011",
                           },
                           new GeoFips
                           {
                               Description = "4 Arlington, VA ",
                               Val = "51013",
                           },
                           new GeoFips
                           {
                               Description = "4 Bath, VA ",
                               Val = "51017",
                           },
                           new GeoFips
                           {
                               Description = "4 Bland, VA ",
                               Val = "51021",
                           },
                           new GeoFips
                           {
                               Description = "4 Botetourt, VA ",
                               Val = "51023",
                           },
                           new GeoFips
                           {
                               Description = "4 Brunswick, VA ",
                               Val = "51025",
                           },
                           new GeoFips
                           {
                               Description = "4 Buchanan, VA ",
                               Val = "51027",
                           },
                           new GeoFips
                           {
                               Description = "4 Buckingham, VA ",
                               Val = "51029",
                           },
                           new GeoFips
                           {
                               Description = "4 Caroline, VA ",
                               Val = "51033",
                           },
                           new GeoFips
                           {
                               Description = "4 Charles City, VA ",
                               Val = "51036",
                           },
                           new GeoFips
                           {
                               Description = "4 Charlotte, VA ",
                               Val = "51037",
                           },
                           new GeoFips
                           {
                               Description = "4 Chesterfield, VA ",
                               Val = "51041",
                           },
                           new GeoFips
                           {
                               Description = "4 Clarke, VA ",
                               Val = "51043",
                           },
                           new GeoFips
                           {
                               Description = "4 Craig, VA ",
                               Val = "51045",
                           },
                           new GeoFips
                           {
                               Description = "4 Culpeper, VA ",
                               Val = "51047",
                           },
                           new GeoFips
                           {
                               Description = "4 Cumberland, VA ",
                               Val = "51049",
                           },
                           new GeoFips
                           {
                               Description = "4 Dickenson, VA ",
                               Val = "51051",
                           },
                           new GeoFips
                           {
                               Description = "4 Essex, VA ",
                               Val = "51057",
                           },
                           new GeoFips
                           {
                               Description = "4 Fauquier, VA ",
                               Val = "51061",
                           },
                           new GeoFips
                           {
                               Description = "4 Floyd, VA ",
                               Val = "51063",
                           },
                           new GeoFips
                           {
                               Description = "4 Fluvanna, VA ",
                               Val = "51065",
                           },
                           new GeoFips
                           {
                               Description = "4 Franklin, VA ",
                               Val = "51067",
                           },
                           new GeoFips
                           {
                               Description = "4 Giles, VA ",
                               Val = "51071",
                           },
                           new GeoFips
                           {
                               Description = "4 Gloucester, VA ",
                               Val = "51073",
                           },
                           new GeoFips
                           {
                               Description = "4 Goochland, VA ",
                               Val = "51075",
                           },
                           new GeoFips
                           {
                               Description = "4 Grayson, VA ",
                               Val = "51077",
                           },
                           new GeoFips
                           {
                               Description = "4 Greene, VA ",
                               Val = "51079",
                           },
                           new GeoFips
                           {
                               Description = "4 Halifax, VA ",
                               Val = "51083",
                           },
                           new GeoFips
                           {
                               Description = "4 Hanover, VA ",
                               Val = "51085",
                           },
                           new GeoFips
                           {
                               Description = "4 Henrico, VA ",
                               Val = "51087",
                           },
                           new GeoFips
                           {
                               Description = "4 Highland, VA ",
                               Val = "51091",
                           },
                           new GeoFips
                           {
                               Description = "4 Isle of Wight, VA ",
                               Val = "51093",
                           },
                           new GeoFips
                           {
                               Description = "4 King and Queen, VA ",
                               Val = "51097",
                           },
                           new GeoFips
                           {
                               Description = "4 King George, VA ",
                               Val = "51099",
                           },
                           new GeoFips
                           {
                               Description = "4 King William, VA ",
                               Val = "51101",
                           },
                           new GeoFips
                           {
                               Description = "4 Lancaster, VA ",
                               Val = "51103",
                           },
                           new GeoFips
                           {
                               Description = "4 Lee, VA ",
                               Val = "51105",
                           },
                           new GeoFips
                           {
                               Description = "4 Loudoun, VA ",
                               Val = "51107",
                           },
                           new GeoFips
                           {
                               Description = "4 Louisa, VA ",
                               Val = "51109",
                           },
                           new GeoFips
                           {
                               Description = "4 Lunenburg, VA ",
                               Val = "51111",
                           },
                           new GeoFips
                           {
                               Description = "4 Madison, VA ",
                               Val = "51113",
                           },
                           new GeoFips
                           {
                               Description = "4 Mathews, VA ",
                               Val = "51115",
                           },
                           new GeoFips
                           {
                               Description = "4 Mecklenburg, VA ",
                               Val = "51117",
                           },
                           new GeoFips
                           {
                               Description = "4 Middlesex, VA ",
                               Val = "51119",
                           },
                           new GeoFips
                           {
                               Description = "4 Nelson, VA ",
                               Val = "51125",
                           },
                           new GeoFips
                           {
                               Description = "4 New Kent, VA ",
                               Val = "51127",
                           },
                           new GeoFips
                           {
                               Description = "4 Northampton, VA ",
                               Val = "51131",
                           },
                           new GeoFips
                           {
                               Description = "4 Northumberland, VA ",
                               Val = "51133",
                           },
                           new GeoFips
                           {
                               Description = "4 Nottoway, VA ",
                               Val = "51135",
                           },
                           new GeoFips
                           {
                               Description = "4 Orange, VA ",
                               Val = "51137",
                           },
                           new GeoFips
                           {
                               Description = "4 Page, VA ",
                               Val = "51139",
                           },
                           new GeoFips
                           {
                               Description = "4 Patrick, VA ",
                               Val = "51141",
                           },
                           new GeoFips
                           {
                               Description = "4 Powhatan, VA ",
                               Val = "51145",
                           },
                           new GeoFips
                           {
                               Description = "4 Prince Edward, VA ",
                               Val = "51147",
                           },
                           new GeoFips
                           {
                               Description = "4 Pulaski, VA ",
                               Val = "51155",
                           },
                           new GeoFips
                           {
                               Description = "4 Rappahannock, VA ",
                               Val = "51157",
                           },
                           new GeoFips
                           {
                               Description = "4 Richmond, VA ",
                               Val = "51159",
                           },
                           new GeoFips
                           {
                               Description = "4 Russell, VA ",
                               Val = "51167",
                           },
                           new GeoFips
                           {
                               Description = "4 Scott, VA ",
                               Val = "51169",
                           },
                           new GeoFips
                           {
                               Description = "4 Shenandoah, VA ",
                               Val = "51171",
                           },
                           new GeoFips
                           {
                               Description = "4 Smyth, VA ",
                               Val = "51173",
                           },
                           new GeoFips
                           {
                               Description = "4 Stafford, VA ",
                               Val = "51179",
                           },
                           new GeoFips
                           {
                               Description = "4 Surry, VA ",
                               Val = "51181",
                           },
                           new GeoFips
                           {
                               Description = "4 Sussex, VA ",
                               Val = "51183",
                           },
                           new GeoFips
                           {
                               Description = "4 Tazewell, VA ",
                               Val = "51185",
                           },
                           new GeoFips
                           {
                               Description = "4 Warren, VA ",
                               Val = "51187",
                           },
                           new GeoFips
                           {
                               Description = "4 Westmoreland, VA ",
                               Val = "51193",
                           },
                           new GeoFips
                           {
                               Description = "4 Wythe, VA ",
                               Val = "51197",
                           },
                           new GeoFips
                           {
                               Description = "4 Alexandria (Independent City), VA ",
                               Val = "51510",
                           },
                           new GeoFips
                           {
                               Description = "4 Chesapeake (Independent City), VA ",
                               Val = "51550",
                           },
                           new GeoFips
                           {
                               Description = "4 Hampton (Independent City), VA ",
                               Val = "51650",
                           },

                       };
                return _values;
            }
        }
	}//end GeoFips
}//end NoFuture.Rand.Gov.Bea.Parameters.RegionalData