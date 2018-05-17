/*
 * jQuery AutoComplete
 *
 * Author: luq885
 * http://blog.csdn.net/luq885 (chinese) 
 *
 * Licensed like jQuery, see http://docs.jquery.com/License
 *
 * 作者：天天无用
 * blog: http://blog.csdn.net/luq885
 *==============================================================
 * 修改者：laolaowhn
 * blig:http://blog.csdn.net/laolaowhn/
 *==============================================================
 */
 
var currentInput;
var key = "";
var isShow = false;
var activeDiv;

var dataRequest = "";//返回数据的分隔符
var kwlength;//关键字最小长度
var autoTab;//是否回车后自动转到下一个文本框
var parameterName;//回传时的参数名
var parameterNameTable;
var parameterNameField; 



jQuery.fn.AutoComplete = function(request, option){
//    if(!document.getElementById("floor"))
//        {
//            document.getElementById("floor").outerHTML="";
//        }
    var selectedDiv=null;//kkai添加
    this.each(function(){        
        if(this.tagName.toLowerCase() == "input" && $(this).attr("type").toLowerCase() == "text")
        {
            $(this).keydown(function(e){
           
                selectText(e.keyCode,this);
            });            
            $(this).keyup(function(e){
                searchKey(e.keyCode);         
            });
            $(this).blur(function()
            {
                /*这里被kkai改过*/
                    
                selectedDiv = $("#floor>div[@class=selected]");
//                if(selectedDiv===null||selectedDiv ==undefined)
//                {
//                    hideText();
//                    return;
//                }
//                else
//                { 

                    if(selectedDiv.length<1)
                    { 
                        hideText(); 
                        return;    
                    }
                    else if(selectedDiv.length==1)/*这里是kkai添加的，防止方向键选择了一个选中项，而，鼠标移开输入框导致的弹出框不隐藏*/
                    {
                        currentInput.val(selectedDiv.text());
                        hideText();
                    }
//               }
            });
        }
    });    
    if(request.length == 0) throw "request is required";
    dataRequest = request;
    kwlength = option.kwlength || 1 ;
    seperator = option.seperator || ",";
    autoTab = option.autoTab || false;
    parameterName = option.parameterName || ""; 
    parameterNameTable=option.parameterNameTable || "";
    parameterNameField=option.parameterNameField || "";
    
    
    //alert(option.parameterName);
    //alert(parameterNameTable);
    //alert(parameterNameField);
    if($("#floor").length<1)//kkai添加，只创建一个floor div
    {
        $("body").prepend("<div id='floor' class='floor'></div>");
    }
    $("#floor").hide();
}

function showText()
{
    isShow=true;
    text = document.getElementById(currentInput.attr("id"));
    div = document.getElementById("floor");
    div.style.left = getPos(text,"Left") + "px";
    div.style.top = getPos(text,"Top") + text.offsetHeight +3+ "px";
    div.style.width = text.offsetWidth - 2 +100+ "px";
    $("#floor").show();
    // parent.document.all("mainFrame").style.height= document.body.scrollHeight+100;
     
}

function hideText()
{
    $("#floor").hide();
    $("#floor").html("");
   
    key="";  
    selectedDiv=null
    //currentInput = null;
    isShow = false;
}

function getPos(el,ePro)				
{
    var ePos=0;
    while(el!=null)
    {		
        ePos+=el["offset"+ePro];
        el=el.offsetParent;
    }
    return ePos;
}

function searchKey(keycode)
{
    if(keycode == 38 || keycode == 40 || keycode == 13 || keycode == 27 || keycode == 9) return;    
    if(currentInput != null && (key == "" || currentInput.val() != key) && currentInput.val().length >= kwlength)
    {    
        var divs = "";
        jQuery.ajax({
            type: "Post",
            dataType: "text",
            url: dataRequest,
            data: parameterNameTable + "&" + parameterNameField + "&" + (parameterName != "" ? parameterName + "=" + currentInput.val() : (currentInput.attr("name") == null ? currentInput.attr("id") + "=" + currentInput.val() : currentInput.serialize())),
            success: function(msg) {
                //alert(encodeURI(currentInput.val()));
                if (msg.length == 0) {
                    hideText();
                    return;
                }
                var datas = msg.split(seperator);
                $.each(datas, function(i, n) {
                    if (n.length > 0) divs += "<div class=unselected onclick=setInput() onmouseout = $(this).attr('class','unselected') onmouseover = mouseover(this)>" + n + "</div>";
                });
                $("#floor").html(divs);
                isShow = true;
                showText();

            }
        });
        key = currentInput.val();
    }    
    if(key.length == 0 || key.length < kwlength) hideText();
    
}
function setInput()
{
    selectedDiv = $("#floor>div[@class=selected]");
    if(selectedDiv.length<1)
    {
        hideText();
        return;
    }
    currentInput.val(selectedDiv.text());
    hideText();
   
}
function findNextInput(target)
{   
    var index;
    $("input[@type=text]").each(function(i){
        if($(this).attr("id") == target.attr("id")) index = i;
    });
    return $("input[@type=text]")[ index + 1 ];
}

function selectText(keycode,sInput)
{     
    var  e=e||window.event;
    currentInput = $("#"+sInput.id);
    if(keycode == 13)
    {        
         //if(autoTab) $(findNextInput(currentInput)).focus();
         //hideText();
         setInput();

         //--------------------------------------回车直接搜索
         if (currentInput.val() != "") {//当搜索框不为空
         	
         	$(".srh_onesearch").click();
         	//$("#a_Seach").click();
         }
         //----------------------------------------
         e.returnValue=false;
            
    }
    if(!isShow) return;
    if(keycode == 27) hideText();   
    if($("#floor>div[@class=selected]")===null||$("#floor>div[@class=selected]")==undefined) 
    {
        selectedDiv=0;
    }
    else
    {        
        selectedDiv = $("#floor>div[@class=selected]");
        
    }
    
    if(selectedDiv.text() != "")
    {
   // $("#testautocomplete3").text($("#testautocomplete3").text()+1);
        selectedDiv.attr("class","unselected");
        if(keycode == 38)
        {
            $("#floor").children("div").attr("class","unselected");
            if(selectedDiv.prev().text() != "")
            {
                 
                selectedDiv.prev().attr("class","selected");
             
                
                //currentInput.val(selectedDiv.prev().text());
            }
            else
            {
                $("#floor>div:last").attr("class","selected");
                //currentInput.val($("#floor>div:last").text());                
            }
            
        }
        else if(keycode == 40)
        {
            $("#floor").children("div").attr("class","unselected");
            if(selectedDiv.next().text() != "")
            {
                selectedDiv.next().attr("class","selected");
               // currentInput.val(selectedDiv.next().text());
            }
            else
            {
                $("#floor>div:first").attr("class","selected");
                //currentInput.val($("#floor>div:first").text());                
            }
           
        }            
    }
    else if(keycode == 38)
    {
        
        $("#floor").children("div").attr("class","unselected");
        $("#floor>div:last").attr("class","selected");
        //currentInput.val($("#floor>div:last").text());
        
    }
    else if(keycode == 40)
    {
        $("#floor").children("div").attr("class","unselected");
        $("#floor>div:first").attr("class","selected");
        //currentInput.val($("#floor>div:first").text()); 
              
    }
}

function mouseover(sDiv)
{    
    $("#floor").children("div").attr("class","unselected");
    $(sDiv).attr("class","selected");
    //currentInput.val($(sDiv).text());
}



//$(document).ready(function(){
//////处理文本框为空时，的提示
//$(".srh_onesearch").click(function()
//{
//    if( $.trim( $("#keyword").val())=="")
//    {
//    //alert("empty");
//        $("#keyword").val("请填写搜索关键字");
//        $("#keyword").css("background","#feea83")
//        return;

//    }
//    submitSearchKey();
//});
//$("#keyword").focus(function()
//{ 
//    if($.trim( $("#keyword").val())=="请填写搜索关键字")
//    {
//        $("#keyword").val("");
//        $("#keyword").css("background","#FFFFFF");
//    }  
//}); 
// $("#a_Seach").click(function()
//{
//    if( $.trim( $("#keyword").val())=="")
//    {
//    //alert("empty");
//        $("#keyword").val("请填写搜索关键字");
//        $("#keyword").css("background","#feea83")
//        return;
//        
//    }
//    Search()
//});

//});