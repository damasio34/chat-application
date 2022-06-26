import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  @Input() error: string | null = null;

  form: FormGroup = new FormGroup({
    username: new FormControl(''),
    password: new FormControl(''),
  });

  username: string = '';
  password: string = '';
  errorMsg = '';

  private formSubmitAttempt = false;

  constructor(private fb: FormBuilder, private loginService: LoginService) { }

  ngOnInit(): void {
    this.loginService.logout();
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  public isFieldInvalid(field: string): boolean {
    return (
      (!this.form.get(field)?.valid && this.form.get(field)?.touched) ||
      (this.form.get(field)?.untouched && this.formSubmitAttempt)
    ) ?? true;
  }

  public login(): void {
    if (this.form.valid) {
      this.loginService.login({
        Username: this.form.value.username,
        Password: this.form.value.password
      });
    }
    this.formSubmitAttempt = true;
    this.errorMsg = '';
  }

}
