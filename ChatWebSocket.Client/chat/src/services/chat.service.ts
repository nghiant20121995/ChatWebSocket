import { Injectable } from '@angular/core';
import { HttpService } from 'src/utilities/http-service';
import { Subject } from 'rxjs';
import User from 'src/models/user.model';
import { LoginResponse } from 'src/models/login-response.model';

@Injectable({
    providedIn: 'root',
})
export default class ChatService {
    private chatSubject: Subject<User> = new Subject<User>();
    public chatObservable = this.chatSubject.asObservable();
    public currentUser: LoginResponse | null = null;

    constructor(private httpService: HttpService) {
        let session = localStorage.getItem('chat_session');
        if (session) {
            this.currentUser = JSON.parse(session);
        } else {
            this.currentUser = null;
        }
    }

    activateUser(user: User) {
        this.chatSubject.next(user);
    }
}