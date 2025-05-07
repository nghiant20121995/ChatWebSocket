import { Component, Input, OnInit } from '@angular/core';
import { BaseResponse } from 'src/models/base-response.model';
import { LoginResponse } from 'src/models/login-response.model';
import User, { GroupContact } from 'src/models/user.model';
import ChatService from 'src/services/chat.service';
import UserDataService from 'src/services/userdata.service';
import UserRoomDataService from 'src/services/userRoomData.service';

@Component({
  selector: 'sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  @Input() sideBarType: number = 1;
  groupContact: GroupContact = {};
  currentUser: LoginResponse | null = null;

  constructor(private userDataService: UserDataService, private chatService: ChatService, private userRoomDataService: UserRoomDataService) {
    this.currentUser = this.chatService.currentUser;
  }

  ngOnInit(): void {
    this.userDataService.getUserData().subscribe(
      (res: BaseResponse<Array<User>>) => {
        if (res.Code === 0 && res.Data) {
          // this.users = res.Data.filter(user => user.Id !== this.currentUser?.Id);

          for (let user of res.Data) {
            if (user.Id === this.currentUser?.Id) {
              continue;
            }
            var firstLetter = user.FullName?.charAt(0).toUpperCase();
            if (firstLetter) {
              if (!this.groupContact.hasOwnProperty(firstLetter)) {
                this.groupContact[firstLetter] = [];
              }
              var firsttwoLetters = user.FullName?.substring(0, 2).toUpperCase();
              user.FirsttwoLetters = firsttwoLetters;
              this.groupContact[firstLetter].push(user);
            }
          }
          this.groupContact = Object.keys(this.groupContact).sort().reduce((acc, key) => {
            acc[key] = this.groupContact[key];
            return acc;
          }, {} as GroupContact);
          console.log('groupContact:', this.groupContact);
        }
      },
      (error: Error) => {
        console.error('Error fetching user data:', error);
      }
    );
    this.userRoomDataService.GetRoomByUserAsync(this.currentUser?.Id!).subscribe(
      (res: any) => {
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

  getGroupContactKeys = (groupContact: GroupContact) => {
    return Object.keys(groupContact);
  }
}
