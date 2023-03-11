import { environment } from 'src/environments/environment';
import { UserSession } from '../../client/login/user-session';
import { AuthenticationService } from '../../client/login/authentication.service';
import { Login } from '../../client/login/login';
import { Component, NgZone, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CredentialResponse } from 'google-one-tap';
import { NgxSpinnerService } from 'ngx-spinner';
import { Buffer } from 'buffer';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  validationMessages: any;
  loginForm: FormGroup;
  login: Login;
  googleButton = "google-button";
  accessTokenKey = "access_token";

  constructor(
    private formBuilder: FormBuilder,
    private authenticationService: AuthenticationService,
    private tostrService: ToastrService,
    private spinnerService: NgxSpinnerService,
    private router: Router,
    private ngZone: NgZone)
  {
    this.defineValidationMessages();
  }

  ngOnInit(): void {
    this.initGoogleAuth();
    this.initLoginForm();
  }

  onSubmit(event: SubmitEvent): void {
    event.preventDefault();

    if (this.loginForm.invalid) {
      this.tostrService.error('Verifique os dados de login!');
      return;
    }

    this.login = Object.assign({}, this.login, this.loginForm.value);

    this.authenticationService.authentication(this.login).subscribe({
      next: (response: UserSession) => {
        let userSession = Buffer.from(JSON.stringify(response)).toString("base64");
        localStorage.setItem(this.accessTokenKey,  userSession);
        this.spinnerService.hide();
        this.router.navigate(['']);
      },
      error: ({message}: Error) => {
        this.tostrService.error(message);
        this.spinnerService.hide();
      }
    });
  }

  private defineValidationMessages() {
    this.validationMessages = {
      email: {
        required: 'O email é obrigatório!',
        email: 'O email deve ser um email válido exemplo@exemplo.com!'
      },
      password: {
        required: 'A senha é obrigatória!'
      }
    };
  }

  private initLoginForm(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  private initGoogleAuth(): void {
    // @ts-ignore
    window.onGoogleLibraryLoad = () => {
      // @ts-ignore
      google.accounts.id.initialize({
        client_id: environment.GOOGLE_CLIENT_ID,
        callback: this.handleCredentialResponse.bind(this),
        native_callback: () => { },
        auto_select: false,
        cancel_on_tap_outside: true,

      });
      // @ts-ignore
      google.accounts.id.renderButton(
        // @ts-ignore
        document.getElementById(this.googleButton),
        { theme: 'outline', size: 'large', width: '100%' }
      );
      // @ts-ignore
      google.accounts.id.prompt((notification: PromptMomentNotification) => {});
    };
  }

  handleCredentialResponse(response: CredentialResponse): void {

    this.spinnerService.show();

    this.authenticationService.authenticationWithGoogle(response.credential).subscribe({
      next: (response: UserSession) => {
        let userSession = Buffer.from(JSON.stringify(response)).toString("base64");
        localStorage.setItem(this.accessTokenKey,  userSession);
        this.ngZone.run(() => {
          this.spinnerService.hide();
          this.router.navigate(['']);
        });
      },
      error: ({message}: Error) => {
        this.ngZone.run(() => {
          this.tostrService.error(message);
          this.spinnerService.hide();
        });
      }
    });
  }
}
