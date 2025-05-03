import { Component, Input, OnInit } from '@angular/core';
import { BaseResponse } from 'src/models/base-response.model';
import { LoginResponse } from 'src/models/login-response.model';
import User from 'src/models/user.model';
import ChatService from 'src/services/chat.service';
import UserDataService from 'src/services/userdata.service';

@Component({
  selector: 'sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  @Input() sideBarType: number = 1;
  users: Array<User> = [];
  currentUser: LoginResponse | null = null;

  constructor(private userDataService: UserDataService, private chatService: ChatService) {
    this.currentUser = this.chatService.currentUser;
  }

  ngOnInit(): void {
    this.userDataService.getUserData().subscribe(
      (res: BaseResponse<Array<User>>) => {
        if (res.Code === 0 && res.Data) {
          this.users = res.Data.filter(user => user.Id !== this.currentUser?.Id);
        }
      },
      (error: Error) => {
        console.error('Error fetching user data:', error);
      }
    );
  }
  onUserClick = (user: User) => {
    console.log('User clicked:', user);
    this.chatService.activateUser(user);
  }
}
