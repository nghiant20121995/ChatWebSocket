import { Component, ViewEncapsulation } from '@angular/core';
import { WebSocketService } from 'src/services/websocket.service';

@Component({
  selector: 'chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent {
  public sideBarType: number = 1;
  constructor(private webSocketService: WebSocketService) {
  }

  toggleSidebar = (sideBarType: number) => {
    this.sideBarType = sideBarType;
  }
}
