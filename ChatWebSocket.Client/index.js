var ws = new WebSocket("ws://localhost:5149/ws?token=eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6Im5naGlhbnQyMDEyMTk5NUBnbWFpbC5jb20iLCJVc2VySWQiOiIxMjMiLCJqdGkiOiJlMDViMGQ0YS0xZmJlLTQ4YTYtYWE2OC04ZWNhYmY0MzBlMWIiLCJleHAiOjE3NDYzNDg1ODYsImlzcyI6ImNoYXQtYXBwbGljYXRpb24iLCJhdWQiOiJjaGF0LWFwcGxpY2F0aW9uIn0.TqrWewB2rSHagFwu4LMAbIqYj5PayO2MIkHfLMUMGvpNFbavXVLwKbgxlb8VIev-dfeGzZYiTfqgGZBpus5j2h3cTzYGeG3Cgnm2J3jqEUY19-Zer0mrgwUXH4WhQlXJLlQiVb3amvWpeIeGorEzjoQJnvLAZ_mFYjVEqjkM6LAoTxUE8-qh7lEpcC9xzX37HVsRh51GYkal0GqT9RML1dkIRt8isoO21k9h7uxan2gtfDPQphss4_pPttZMQzCJh8VsCqKs-B7agkuh-UVGB78R9YFDWDUA4-Ux8v7oiJRuOIdcqvaq4tS1JKHw8gDE0mI1e6ep3xX7_DudcaJbVA");
ws.addEventListener("open", event => {

});
ws.addEventListener("close", event => {

});
ws.addEventListener("open", event => {

});
ws.addEventListener("error", event => {

});

ws.addEventListener("message", event => {
    var msg = JSON.parse(event.data);
    console.log(msg.Content);
    var contentElement = document.getElementById("Content");
    var newElement = document.createElement("p");
    newElement.innerHTML = msg.Content;
    contentElement.append(newElement);
});


function OnSendMsg() {
    var json = {
        ReceiverId: "124",
        Content: "Hello this is the first message",
        IsGroup: false
    };
    ws.send(JSON.stringify(json));
}