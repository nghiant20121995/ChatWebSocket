import { Component, ViewEncapsulation } from '@angular/core';
import { WebSocketService } from 'src/services/websocket.service';

@Component({
  selector: 'chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ChatComponent {
  constructor(private webSocketService: WebSocketService) { }
}
