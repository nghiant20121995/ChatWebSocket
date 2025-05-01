import { Injectable } from '@angular/core';
import {
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
    HttpErrorResponse,
    HttpStatusCode
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
    constructor(private router: Router) { }

    private token: string | null = null;

    intercept(
        req: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        // Add Bearer token to the request
        const token = this.getToken(); // Replace with your token retrieval logic
        const clonedRequest = token
            ? req.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            })
            : req;

        return next.handle(clonedRequest).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === HttpStatusCode.Unauthorized) {
                    // Redirect to the login page
                    this.router.navigate(['/login']);
                    return throwError(() => error);
                }
                console.error('HTTP Error:', error);
                // Handle specific error cases here, e.g., show a notification or redirect
                return throwError(() => error);
            })
        );
    }

    private getToken(): string | null {
        if (!this.token) {
            this.token = localStorage.getItem('session_token');
        }
        return this.token;
    }
}