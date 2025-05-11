import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { LoginResponse } from 'src/models/login-response.model';
import Message from 'src/models/message.model';
import { MessageFilterResponse } from 'src/models/messagefilter-response.model';
import User from 'src/models/user.model';
import UserRoom from 'src/models/userRoom.model';
import ChatService from 'src/services/chat.service';
import MessageDataService from 'src/services/messageData.service';
import { WebSocketService } from 'src/services/websocket.service';

@Component({
  selector: 'chat-area',
  templateUrl: './chat-area.component.html',
  styleUrls: ['./chat-area.component.css']
})
export class ChatAreaComponent implements OnInit, OnDestroy {
  private msgObserver?: Subscription;
  public messages: Array<Message> = [];
  public currentUser: LoginResponse | null = null;
  public receiver: User | null = null;

  constructor(private webSocketService: WebSocketService, private chatService: ChatService, private messageDataService: MessageDataService) {
    this.currentUser = this.chatService.currentUser;
    this.chatService.chatObservable.subscribe(this.loadReceiver);
  }
  
  loadReceiver = (user: User | UserRoom) => {
    if (user.Id !== this.currentUser?.Id) {
      this.receiver = <User>user;
      let roomId = [this.currentUser?.Id, user.Id].sort().join('_');
      this.messageDataService.GetByRoomAsync(roomId).subscribe(
        (res: MessageFilterResponse) => {
          if (res) {
            this.messages = res.Data!;
          }
        }
      );
    }
    else if (user instanceof UserRoom) {
      this.receiver = null;
      this.messages = [];
    }
  }

  ngOnDestroy(): void {
    this.msgObserver?.unsubscribe();
    console.log('WebSocket connection closed');
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
        ReceiverId: this.receiver?.Id,
      });
      messageForm.reset();
    }
  }
}
