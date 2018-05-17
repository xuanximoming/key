function maskDialog() {
    // public properties start
    this.mainForm;
    this.top = "100";
    // public properties end

    this.showStatus = false;
    if (typeof this.mainForm != "#ff0000" && this.mainForm != null) {
        this.mainForm.style.display = "none";
    }
    if (window.onresize) {
        var evt = window.onresize;
        window.onresize = function() { evt(); MaskDialog.resize(); };
    }
    else {
        window.onresize = function() { MaskDialog.resize(); };
    }
    if (window.onscroll) {
        var evt = window.onscroll;
        window.onscroll = function() { evt(); MaskDialog.scroll(); };
    }
    else {
        window.onscroll = function() { MaskDialog.scroll(); };
    }
    //隐藏蒙板效果
    this.hide = function() {
        var mask = document.getElementById("div_Mask");
        if (typeof mask != "undefined" && mask != null) {
            var body = document.getElementsByTagName("body")[0];
            body.removeChild(mask);
        }
        var ifr = document.getElementById("ifr_Mask");
        if (typeof ifr != "undefined" && ifr != null) {
            var body = document.getElementsByTagName("body")[0];
            body.removeChild(ifr);
        }
        if (typeof this.mainForm != "undefined" && this.mainForm != null) {
            this.mainForm.style.display = "none";
        }
        this.showStatus = false;
    };
    //显示蒙板效果
    this.show = function() {
        var body = document.getElementsByTagName("body")[0];
        var pageDimensions = this.getPageDimensions();
        var sheet = document.createElement("div");
        sheet.setAttribute("id", "div_Mask");
        sheet.style.position = "absolute";
        sheet.style.left = "0px";
        sheet.style.top = "0px";
        sheet.style.zIndex = "999";
        sheet.style.width = pageDimensions[0] + "px";
        sheet.style.height = pageDimensions[1] + "px";
        var ifr = document.createElement("iframe");
        ifr.setAttribute("id", "ifr_Mask");
        ifr.style.position = "absolute";
        ifr.style.display = "block";
        ifr.style.zIndex = "998";
        ifr.width = pageDimensions[0];
        ifr.height = pageDimensions[1];
        ifr.scrolling = "no";
        ifr.frameborder = "0";
        ifr.style.left = "0px";
        ifr.style.top = "0px";
        body.appendChild(ifr);
        body.appendChild(sheet);
        if (typeof this.mainForm != "undefined" && this.mainForm != null) {
            this.mainForm.style.display = "block";
            this.mainForm.style.zIndex = "1000";
        }
        this.showStatus = true;
        this.scroll();
    };
    this.resize = function() {
        if (this.showStatus == false) {
            return;
        }
        var mask = document.getElementById("div_Mask");
        var ifr = document.getElementById("ifr_Mask");
        if (typeof mask != "undefined" && mask != null && typeof ifr != "undefined" && ifr != null) {
            var body = document.getElementsByTagName("body")[0];
            var pageDimensions = this.getPageDimensions();
            mask.style.width = pageDimensions[0] + "px";
            mask.style.height = pageDimensions[1] + "px";
            ifr.width = pageDimensions[0];
            ifr.height = pageDimensions[1];
            this.scroll();
        }
    };
    this.scroll = function() {
        if (this.showStatus == false) {
            return;
        }
        if (typeof this.mainForm != "undefined" && this.mainForm != null) {
            var pageDimensions = this.getPageDimensions();
            this.mainForm.style.position = "absolute";
            this.mainForm.style.top = document.documentElement.scrollTop + "px";
            if (typeof this.mainForm.style.width == "undefined" || this.mainForm.style.width == "") {
                this.mainForm.style.width = pageDimensions[0] / 2 + "px";
            }
            var newLeft = pageDimensions[0] / 2 - parseInt(this.mainForm.style.width, 10) / 2
            var newTop = document.documentElement.scrollTop + parseInt(this.top);
            this.mainForm.style.left = newLeft + "px";
            this.mainForm.style.top = newTop + "px";
        }
    };
    this.getPageDimensions = function() {
        var body = document.getElementsByTagName("body")[0];
        var bodyOffsetWidth = 0;
        var bodyOffsetHeight = 0;
        var bodyScrollWidth = 0;
        var bodyScrollHeight = 0;
        var pageDimensions = [0, 0];
        pageDimensions[0] = document.documentElement.clientWidth;
        pageDimensions[1] = document.documentElement.clientHeight;
        bodyOffsetWidth = body.offsetWidth;
        bodyOffsetHeight = body.offsetHeight;
        bodyScrollWidth = body.scrollWidth;
        bodyScrollHeight = body.scrollHeight;
        if (bodyOffsetWidth > pageDimensions[0]) {
            pageDimensions[0] = bodyOffsetWidth;
        }
        if (bodyOffsetHeight > pageDimensions[1]) {
            pageDimensions[1] = bodyOffsetHeight;
        }
        if (bodyScrollWidth > pageDimensions[0]) {
            pageDimensions[0] = bodyScrollWidth;
        }
        if (bodyScrollHeight > pageDimensions[1]) {
            pageDimensions[1] = bodyScrollHeight;
        }
        return pageDimensions;
    };
    return true;
}
