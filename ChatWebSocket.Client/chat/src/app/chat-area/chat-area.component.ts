import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import Message from 'src/models/message.model';
import { WebSocketService } from 'src/services/websocket.service';

@Component({
  selector: 'chat-area',
  templateUrl: './chat-area.component.html',
  styleUrls: ['./chat-area.component.css']
})
export class ChatAreaComponent implements OnInit {
  private msgObserver: any;
  public messages: Array<Message> = [];
  public currentUser: any = null;

  constructor(private webSocketService: WebSocketService) {
    let session = localStorage.getItem('chat_session');
    if (session) {
      this.currentUser = JSON.parse(session);
    }
    console.log('Current user:', this.currentUser);
  }

  ngOnInit(): void {
    this.msgObserver = this.webSocketService.getMessages().subscribe(
      (message: Message) => {
        console.log('Message received:', message);
        this.messages.push(message);
        // Handle incoming messages here
      }
    );
  }

  sendMessage = (messageForm: NgForm) => {
    // Logic to send a message
    console.log('Send message button clicked');
    var message = messageForm.value.message;
    if (message) {
      this.webSocketService.sendMessage({
        IsGroup: false,
        Content: message,
        ReceiverId: "124",
      });
      messageForm.reset();
    }
  }
}
