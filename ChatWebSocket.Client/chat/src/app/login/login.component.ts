import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import UserDataService from 'src/services/userdata.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private userDataService: UserDataService, private router: Router) { }
  onLogin(form: NgForm): void {
    // Logic for login action
    console.log('Login button clicked', form.value);
    const response = this.userDataService.login(form.value.email, form.value.password).subscribe(
      (response) => {
        if (response?.Code === 0 && response.Data) {
          localStorage.setItem('chat_session', JSON.stringify(response.Data));
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
