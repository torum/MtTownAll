using System;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MtTownAll.Models;

public partial class NodeMenu : NodeTree
{
    public NodeMenu(string name) : base(name)
    {
        PathIcon = "M20,18H4V8H20M20,6H12L10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6Z";
    }
}

public partial class NodeMenuPrefecture : NodeMenu
{
    public NodeMenuPrefecture(string name) : base(name)
    {
        PathIcon = "M17 5.5A2.5 2.5 0 0 0 14.5 3h-9A2.5 2.5 0 0 0 3 5.5v9A2.5 2.5 0 0 0 5.5 17h4.1a5.5 5.5 0 0 1-.393-1H5.5A1.5 1.5 0 0 1 4 14.5V7h12v2.207q.524.149 1 .393zM5.5 4h9A1.5 1.5 0 0 1 16 5.5V6H4v-.5A1.5 1.5 0 0 1 5.5 4M19 14.5a4.5 4.5 0 1 1-9 0a4.5 4.5 0 0 1 9 0m-2.287-.437l-2.97-1.65a.5.5 0 0 0-.743.437v3.3a.5.5 0 0 0 .743.437l2.97-1.65a.5.5 0 0 0 0-.874";
            //"M19,9H2V11H19V9M19,5H2V7H19V5M2,15H15V13H2V15M17,13V19L22,16L17,13Z";
    }
}

public partial class NodeMenuTest : NodeMenu
{
    public NodeMenuTest(string name) : base(name)
    {
        //PathIcon = "M19.31 18.9L22.39 22L21 23.39L17.88 20.32C17.19 20.75 16.37 21 15.5 21C13 21 11 19 11 16.5C11 14 13 12 15.5 12C18 12 20 14 20 16.5C20 17.38 19.75 18.21 19.31 18.9M15.5 19C16.88 19 18 17.88 18 16.5C18 15.12 16.88 14 15.5 14C14.12 14 13 15.12 13 16.5C13 17.88 14.12 19 15.5 19M21 4V6H3V4H21M3 16V14H9V16H3M3 11V9H21V11H18.97C17.96 10.37 16.77 10 15.5 10C14.23 10 13.04 10.37 12.03 11H3Z";
        PathIcon = "M11 18.95C7.77 18.72 6 17.45 6 17V14.77C7.13 15.32 8.5 15.69 10 15.87C10 15.21 10.04 14.54 10.21 13.89C8.5 13.67 6.97 13.16 6 12.45V9.64C7.43 10.45 9.5 10.97 11.82 11C11.85 10.97 11.87 10.93 11.9 10.9C14.1 8.71 17.5 8.41 20 10.03V7C20 4.79 16.42 3 12 3S4 4.79 4 7V17C4 19.21 7.59 21 12 21C12.34 21 12.68 21 13 20.97C12.62 20.72 12.24 20.44 11.9 20.1C11.55 19.74 11.25 19.36 11 18.95M12 5C15.87 5 18 6.5 18 7S15.87 9 12 9 6 7.5 6 7 8.13 5 12 5M20.31 17.9C20.75 17.21 21 16.38 21 15.5C21 13 19 11 16.5 11S12 13 12 15.5 14 20 16.5 20C17.37 20 18.19 19.75 18.88 19.32L22 22.39L23.39 21L20.31 17.9M16.5 18C15.12 18 14 16.88 14 15.5S15.12 13 16.5 13 19 14.12 19 15.5 17.88 18 16.5 18Z";
    }
}

public partial class NodeMenuTownAll : NodeMenu
{
    public NodeMenuTownAll(string name) : base(name)
    {
        //PathIcon = "M19.31 18.9L22.39 22L21 23.39L17.88 20.32C17.19 20.75 16.37 21 15.5 21C13 21 11 19 11 16.5C11 14 13 12 15.5 12C18 12 20 14 20 16.5C20 17.38 19.75 18.21 19.31 18.9M15.5 19C16.88 19 18 17.88 18 16.5C18 15.12 16.88 14 15.5 14C14.12 14 13 15.12 13 16.5C13 17.88 14.12 19 15.5 19M21 4V6H3V4H21M3 16V14H9V16H3M3 11V9H21V11H18.97C17.96 10.37 16.77 10 15.5 10C14.23 10 13.04 10.37 12.03 11H3Z";
        PathIcon = "M11 18.95C7.77 18.72 6 17.45 6 17V14.77C7.13 15.32 8.5 15.69 10 15.87C10 15.21 10.04 14.54 10.21 13.89C8.5 13.67 6.97 13.16 6 12.45V9.64C7.43 10.45 9.5 10.97 11.82 11C11.85 10.97 11.87 10.93 11.9 10.9C14.1 8.71 17.5 8.41 20 10.03V7C20 4.79 16.42 3 12 3S4 4.79 4 7V17C4 19.21 7.59 21 12 21C12.34 21 12.68 21 13 20.97C12.62 20.72 12.24 20.44 11.9 20.1C11.55 19.74 11.25 19.36 11 18.95M12 5C15.87 5 18 6.5 18 7S15.87 9 12 9 6 7.5 6 7 8.13 5 12 5M20.31 17.9C20.75 17.21 21 16.38 21 15.5C21 13 19 11 16.5 11S12 13 12 15.5 14 20 16.5 20C17.37 20 18.19 19.75 18.88 19.32L22 22.39L23.39 21L20.31 17.9M16.5 18C15.12 18 14 16.88 14 15.5S15.12 13 16.5 13 19 14.12 19 15.5 17.88 18 16.5 18Z";
    }
}

public partial class NodeMenuPostalCode : NodeMenu
{
    public NodeMenuPostalCode(string name) : base(name)
    {
        //PathIcon = "M19.31 18.9L22.39 22L21 23.39L17.88 20.32C17.19 20.75 16.37 21 15.5 21C13 21 11 19 11 16.5C11 14 13 12 15.5 12C18 12 20 14 20 16.5C20 17.38 19.75 18.21 19.31 18.9M15.5 19C16.88 19 18 17.88 18 16.5C18 15.12 16.88 14 15.5 14C14.12 14 13 15.12 13 16.5C13 17.88 14.12 19 15.5 19M21 4V6H3V4H21M3 16V14H9V16H3M3 11V9H21V11H18.97C17.96 10.37 16.77 10 15.5 10C14.23 10 13.04 10.37 12.03 11H3Z";
        PathIcon = "M11 18.95C7.77 18.72 6 17.45 6 17V14.77C7.13 15.32 8.5 15.69 10 15.87C10 15.21 10.04 14.54 10.21 13.89C8.5 13.67 6.97 13.16 6 12.45V9.64C7.43 10.45 9.5 10.97 11.82 11C11.85 10.97 11.87 10.93 11.9 10.9C14.1 8.71 17.5 8.41 20 10.03V7C20 4.79 16.42 3 12 3S4 4.79 4 7V17C4 19.21 7.59 21 12 21C12.34 21 12.68 21 13 20.97C12.62 20.72 12.24 20.44 11.9 20.1C11.55 19.74 11.25 19.36 11 18.95M12 5C15.87 5 18 6.5 18 7S15.87 9 12 9 6 7.5 6 7 8.13 5 12 5M20.31 17.9C20.75 17.21 21 16.38 21 15.5C21 13 19 11 16.5 11S12 13 12 15.5 14 20 16.5 20C17.37 20 18.19 19.75 18.88 19.32L22 22.39L23.39 21L20.31 17.9M16.5 18C15.12 18 14 16.88 14 15.5S15.12 13 16.5 13 19 14.12 19 15.5 17.88 18 16.5 18Z";
    }
}

//MenuTreeItem_PostalCode
public partial class MenuTreeBuilder : NodeTree
{
    public MenuTreeBuilder(string name) : base(name)
    {
        ResourceLoader resourceLoader = new();

        var str = resourceLoader.GetString("MenuTreeItem_Prefecture");
        NodeMenuPrefecture nmpref = new(str)
        {
            Tag = "Prefecture",
            Selected = false,
            Expanded = false,

            Parent = this
        };
        Children.Add(nmpref);


        str = resourceLoader.GetString("MenuTreeItem_TownAll");
        NodeMenuTownAll nmta = new(str)
        {
            Tag = "TownAll",
            Selected = false,
            Expanded = false,

            Parent = this
        };
        Children.Add(nmta);


        str = resourceLoader.GetString("MenuTreeItem_PostalCode");
        NodeMenuPostalCode nmpc = new(str)
        {
            Tag = "PostalCode",
            //Selected = true,
            Selected = false,
            Expanded = false,

            Parent = this
        };
        Children.Add(nmpc);

        // TODO:
        /*
        str = resourceLoader.GetString("MenuTreeItem_Test");
        NodeMenuTest nmt = new(str)
        {
            Tag = "Test",
            //Selected = true,
            Selected = false,
            Expanded = false,

            Parent = this
        };
        Children.Add(nmt);
        */
    }
}
