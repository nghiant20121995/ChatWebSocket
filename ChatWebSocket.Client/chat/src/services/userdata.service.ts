import { Injectable } from '@angular/core';
import { HttpService } from 'src/utilities/http-service';
import { environment } from 'src/environments/environment';
import { LoginResponseBase } from 'src/models/login-response.model';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root',
})
export default class UserDataService {
    private apiUrl = environment.apiUrl;

    constructor(private httpService: HttpService) { }

    // Example method to get user data
    getUserData(): any {
        // Replace with actual implementation
        return this.httpService.get(`${this.apiUrl}/user`);
    }

    // Example method to update user data
    updateUserData(userData: any): void {
        // Replace with actual implementation
        console.log('User data updated:', userData);
    }

    login(email: string, password: string): Observable<LoginResponseBase> {
        // Use the backend URL from the environment file
        var header = new HttpHeaders();
        return this.httpService.post<LoginResponseBase>(
            `${this.apiUrl}/login`,
            { email, password }
        );
    }
}