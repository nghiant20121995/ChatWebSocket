import { Injectable } from '@angular/core';
import { HttpService } from 'src/utilities/http-service';
import { environment } from 'src/environments/environment';
import { LoginResponseBase } from 'src/models/login-response.model';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    private backendUrl = environment.backendUrl;

    constructor(private httpService: HttpService) {}

    // Example method to get user data
    getUserData(): any {
        // Replace with actual implementation
        return this.httpService.get(`${this.backendUrl}/api/user`);
    }

    // Example method to update user data
    updateUserData(userData: any): void {
        // Replace with actual implementation
        console.log('User data updated:', userData);
    }

    login(username: string, password: string): Observable<LoginResponseBase> {
        // Use the backend URL from the environment file
        return this.httpService.post<LoginResponseBase>(`${this.backendUrl}/api/login`, { username, password });
    }
}