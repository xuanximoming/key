

$(document).ready(function(){
////处理文本框为空时，的提示
//$(".srh_onesearch").click(function()
//{
//    if( $.trim( $("#keyword").val())=="" || $.trim( $("#keyword").val())=="请填写搜索关键字")
//    {
//    //alert("empty");
//        $("#keyword").val("请填写搜索关键字");
//        $("#keyword").css("background","#cafeff")
//        $("#keyword").blur();
//        return;

//    }
//    submitSearchKey();
//});
$("#keyword").focus(function()
{ 
    if($.trim( $("#keyword").val())=="请填写搜索关键字" )
    {
        $("#keyword").val("");
        $("#keyword").css("background","#FFFFFF");
    }  
}); 
 $("#a_Seach").click(function()
{
    if( $.trim( $("#keyword").val())=="" || $.trim( $("#keyword").val())=="请填写搜索关键字")
    {
    //alert("empty");
        $("#keyword").val("请填写搜索关键字");
        $("#keyword").css("background","#cafeff")
        $("#keyword").blur();
        return;
        
    }
    Search()
});

});