// websocket.service.ts
import { Injectable } from '@angular/core';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class WebSocketService {
    private socket$: WebSocketSubject<any>;

    constructor() {
        let sessionToken = localStorage.getItem('session_token');
        let wsUrl = environment.websocketUrl + '?token=' + sessionToken;
        this.socket$ = webSocket(wsUrl);
    }

    sendMessage(msg: any): void {
        this.socket$.next(msg);
    }

    getMessages(): Observable<any> {
        return this.socket$.asObservable();
    }

    close(): void {
        this.socket$.complete(); // gracefully close the connection
    }
    reconnect(): void {
        this.close(); // close the existing connection
        let sessionToken = localStorage.getItem('session_token');
        let wsUrl = environment.websocketUrl + '?token=' + sessionToken;
        this.socket$ = webSocket(wsUrl);
    }
}
