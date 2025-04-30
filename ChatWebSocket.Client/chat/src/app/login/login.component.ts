import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private userService: UserService) {}
  onLogin(form: NgForm): void {
    // Logic for login action
    console.log('Login button clicked', form.value);
    const response = this.userService.login(form.value.email, form.value.password).subscribe(
      (response) => {
        console.log('Login successful', response);
        // Handle successful login, e.g., navigate to another page or show a success message
      },
      (error) => {
        console.error('Login failed', error);
        // Handle login failure, e.g., show an error message
      }
    );
    console.log('Response:', response);
  }
}
