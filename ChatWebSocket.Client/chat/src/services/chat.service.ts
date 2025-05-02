import { Injectable } from '@angular/core';
import { HttpService } from 'src/utilities/http-service';
import { Subject } from 'rxjs';
import User from 'src/models/user.model';

@Injectable({
    providedIn: 'root',
})
export default class ChatService {
    private chatSubject: Subject<User> = new Subject<User>();
    public chatObservable = this.chatSubject.asObservable();

    constructor(private httpService: HttpService) {
    }

    activateUser(user: User) {
        this.chatSubject.next(user);
    }
}