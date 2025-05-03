import { Injectable } from '@angular/core';
import { HttpService } from 'src/utilities/http-service';
import { environment } from 'src/environments/environment';
import { LoginResponseBase } from 'src/models/login-response.model';
import { Observable } from 'rxjs';
import { MessageFilterResponse } from 'src/models/messagefilter-response.model';

@Injectable({
    providedIn: 'root',
})
export default class MessageDataService {
    private apiUrl = environment.apiUrl;

    constructor(private httpService: HttpService) {}

    // Example method to get user data
    GetByRoomAsync(RoomId: string, FromDate: Date | null = null, ToDate: Date | null = null, SenderId: string | null = null, ReceiverId: string | null = null): Observable<MessageFilterResponse> {
        // Replace with actual implementation
        var url = `${this.apiUrl}/message?RoomId=${RoomId}`;
        if (FromDate) {
            url += `&FromDate=${FromDate.toISOString()}`;
        }
        if (ToDate) {
            url += `&ToDate=${ToDate.toISOString()}`;
        }
        if (SenderId) {
            url += `&SenderId=${SenderId}`;
        }
        if (ReceiverId) {
            url += `&ReceiverId=${ReceiverId}`;
        }

        return this.httpService.get<MessageFilterResponse>(url);
    }
}