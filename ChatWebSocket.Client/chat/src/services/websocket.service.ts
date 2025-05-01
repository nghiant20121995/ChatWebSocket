// websocket.service.ts
import { Injectable, OnDestroy } from '@angular/core';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class WebSocketService implements OnDestroy {
    private socket$: WebSocketSubject<any>;

    constructor() {
        let chatSession = localStorage.getItem('chat_session');
        let sessionToken = chatSession ? JSON.parse(chatSession).Token : null;
        let wsUrl = environment.websocketUrl + '?token=' + sessionToken;
        this.socket$ = webSocket(wsUrl);
        this.socket$.subscribe(
            (message) => {
                console.log('WebSocket message received:', message);
            },
            (error) => {
                console.error('WebSocket error:', error);
            },
            () => {
                console.log('WebSocket connection closed');
            }
        );
    }
    ngOnDestroy(): void {
        this.socket$.unsubscribe();
        this.socket$.complete(); // close the socket when the service is destroyed
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
        let chatSession = localStorage.getItem('chat_session');
        let sessionToken = chatSession ? JSON.parse(chatSession).token : null;
        let wsUrl = environment.websocketUrl + '?token=' + sessionToken;
        this.socket$ = webSocket(wsUrl);
    }
}
