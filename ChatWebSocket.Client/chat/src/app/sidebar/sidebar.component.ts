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
  users = [
    {
      name: "Alice",
      avatar: "https://i.pravatar.cc/150?img=5",
      messages: [
        { from: "Alice", text: "Hey there! 👋" },
        { from: "me", text: "Hi Alice! How's it going?" },
        { from: "Alice", text: "Pretty good, just chilling 😎" },
        { from: "me", text: "Nice! 👍" },
        { from: "Alice", text: "Reacted: ❤️" }
      ]
    },
    {
      name: "Bob",
      avatar: "https://i.pravatar.cc/150?img=6",
      messages: [
        { from: "Bob", text: "Brooo did you see the match? 🔥" },
        { from: "me", text: "Insane! 😂 That last goal!" },
        { from: "Bob", text: "Best game ever ✅" },
        { from: "me", text: "Reacted: 😂" }
      ]
    },
    {
      name: "Charlie",
      avatar: "https://i.pravatar.cc/150?img=7",
      messages: [
        { from: "Charlie", text: "Deadline is tomorrow!! ⏰" },
        { from: "me", text: "On it boss 🙌" },
        { from: "Charlie", text: "Reacted: 👍" }
      ]
    }
  ];

  currentUser = null;

  ngOnInit(): void {
    this.userService.getUserData().subscribe(
      (res: BaseResponse<Array<User>>) => {
        if (res.code === 0) {
          // this.users = res.data.map(user => ({
          //   name: user.FullName,
          //   avatar: user.Avatar,
          //   messages: [
          //     { from: user.FullName, text: "Hey there! 👋" },
          //     { from: "me", text: "Hi " + user.FullName + "! How's it going?" },
          //     { from: user.FullName, text: "Pretty good, just chilling 😎" },
          //     { from: "me", text: "Nice! 👍" },
          //     { from: user.FullName, text: "Reacted: ❤️" }
          //   ]
          // }));
        }
      },
      (error: Error) => {
        console.error('Error fetching user data:', error);
      }
    );
  }
}
