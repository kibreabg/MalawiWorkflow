
function Clickheretoprint(theid) 
{
    var disp_setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
    disp_setting += "scrollbars=yes,width=750, height=600, left=100, top=25";
    var content_vlue = document.getElementById(theid).innerHTML;

    var docprint = window.open("", "", disp_setting);
    docprint.document.open();
    docprint.document.write('<html><head><title>CHAI ZIM</title>');
    docprint.document.write('</head><body onLoad="self.print()"><center>');
    docprint.document.write(content_vlue);
    docprint.document.write('</center></body></html>');
    docprint.document.close();
    docprint.focus();
}

var collapseImage = "/Modules/Forlab/Images/collapsed.gif";
var expandImage = "/Modules/Forlab/Images/expanded.gif";

function ExpandCollapse(imageItem, sectionId) {
    if (ItemCollapsed(sectionId) == true) {
        imageItem.src = expandImage;
        imageItem.alt = "Collaps";

        ExpandSection(sectionId);
    }
    else {
        imageItem.src = collapseImage;
        imageItem.alt = "Expand";

        CollapseSection(sectionId);
    }

}

function ExpandSection(sectionId) {
    try {
        var sectionElement = document.getElementById("keySection" + sectionId);
        sectionElement.style.display = "";
    }
    catch (e) {
    }
}

function CollapseSection(sectionId) {
    try {
        var sectionElement = GetDiv("keySection" + sectionId);
        sectionElement.style.display = "none";
    }
    catch (e) {
    }
}

function ItemCollapsed(sectionId) {
    //var sectionElement = document.getElementById("keySection" + sectionId);
    var sectionElement = GetDiv("keySection" + sectionId);
    if (sectionElement == null) return;

    return sectionElement.style.display == "none";
}

function GetDiv(sDiv) {
    var div;
    if (document.getElementById)
        div = document.getElementById(sDiv);
    else if (document.all)
        div = eval("window." + sDiv);
    else if (document.layers)
        div = document.layers[sDiv];
    else
        div = null;
    return div;
}

function CheckUncheckAll(chbItem, sectionId) {
    var sectionElement = document.getElementById("keySection" + sectionId);
    var length = sectionElement.children.length;
    var i;

    for (i = 0; i < length; ++i) {
        var e = sectionElement.children[i];

        if (e != null && e.tagName != null &&
	        e.tagName.toLowerCase() == "input" &&
	        e.type.toLowerCase() == "checkbox") {
            e.checked = chbItem.checked;
        }
    }
}

function CheckUncheckGroup(chbItem) {
    if (chbItem.checked) return;

    var section = chbItem.parentElement.parentElement;
    var length = section.children.length;
    var i;
    for (i = 0; i < length; ++i) {
        var e = section.children[i];
        if (e != null && e.tagName != null && e.tagName.toLowerCase() == "input" && e.type.toLowerCase() == "checkbox") {
            e.checked = false;
        }
    }
}