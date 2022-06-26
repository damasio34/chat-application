import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Login } from './login';
import { LoginResult } from './login-result';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private storageKey = {
    username: 'chat-application-username',
    token: 'chat-application-token',
  };

  private loggedIn = new BehaviorSubject<boolean>(false);

  get isLoggedIn(): Observable<boolean> {
    return this.loggedIn.asObservable();
  }

  get token(): string {
    return localStorage.getItem(this.storageKey.token)!;
  }

  get username(): string {
    return localStorage.getItem(this.storageKey.username)!;
  }

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient, private router: Router) {
    if (localStorage.getItem(this.storageKey.token)) {
      this.loggedIn.next(!!localStorage.getItem(this.storageKey.token));
    }
  }

  public login(login: Login): void {
    this.http.post<LoginResult>(`${environment.apiUrl}/auth/login`, login, this.httpOptions)
      .subscribe({
        next: (loginResult: LoginResult) => {
          localStorage.setItem(this.storageKey.token, loginResult.token);
          localStorage.setItem(this.storageKey.username, loginResult.username);
          this.loggedIn.next(true);
          this.router.navigate(['/']);
        }
      });
  }

  public logout(): void {
    localStorage.removeItem(this.storageKey.token);
    localStorage.removeItem(this.storageKey.username);
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }
}
