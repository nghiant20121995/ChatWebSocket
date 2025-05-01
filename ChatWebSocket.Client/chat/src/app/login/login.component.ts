import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private userService: UserService, private router: Router) { }
  onLogin(form: NgForm): void {
    // Logic for login action
    console.log('Login button clicked', form.value);
    const response = this.userService.login(form.value.email, form.value.password).subscribe(
      (response) => {
        if (response?.data?.token) {
          localStorage.setItem('session_token', response?.data?.token);
          this.router.navigate(['/']);
        }
      },
      (error) => {
        console.error('Login failed', error);
        // Handle login failure, e.g., show an error message
      }
    );
    console.log('Response:', response);
  }
}
