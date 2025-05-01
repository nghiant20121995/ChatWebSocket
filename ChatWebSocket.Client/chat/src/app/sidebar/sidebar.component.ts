import { Component, OnInit } from '@angular/core';
import { BaseResponse } from 'src/models/base-response.model';
import User from 'src/models/user.model';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  constructor(private userService: UserService) { }

  // Dummy users + messages
  users: Array<User> = [];

  currentUser = null;

  ngOnInit(): void {
    this.userService.getUserData().subscribe(
      (res: BaseResponse<Array<User>>) => {
        if (res.Code === 0 && res.Data) {
          this.users = res.Data;
        }
      },
      (error: Error) => {
        console.error('Error fetching user data:', error);
      }
    );
  }
}
