import { Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    RouterLink,
  ],
  templateUrl: './register.component.html',
})
export class RegisterComponent {

  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly isLoading = signal(false);
  readonly errorMessage = signal('');
  readonly submitted = signal(false);

  readonly registerForm = this.fb.nonNullable.group({
    nomeCompleto: this.fb.nonNullable.control('', Validators.required),
    eta: this.fb.nonNullable.control(18, Validators.required),
    email: this.fb.nonNullable.control('', [Validators.required, Validators.email]),
    password: this.fb.nonNullable.control('', [Validators.required, Validators.minLength(6)]),
  });

  submit(): void {
    this.submitted.set(true);

    if (this.registerForm.invalid) {
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    const payload = this.registerForm.getRawValue();

    this.authService.registrazione(payload).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.router.navigate(['/login']);
      },
      error: (err: unknown) => {
        this.isLoading.set(false);
        this.errorMessage.set(this.extractError(err));
      }
    });
  }

  private extractError(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message ?? 'Errore durante la registrazione.';
    }
    return 'Errore imprevisto.';
  }
}
