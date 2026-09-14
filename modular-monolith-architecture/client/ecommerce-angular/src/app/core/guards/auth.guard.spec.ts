import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { signal } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { authGuard } from './auth.guard';

describe('authGuard', () => {
  const authenticated = signal(false);
  const router = { createUrlTree: jasmine.createSpy('createUrlTree').and.returnValue('/login') };

  beforeEach(() => {
    authenticated.set(false);
    router.createUrlTree.calls.reset();
    TestBed.configureTestingModule({ providers: [{ provide: AuthService, useValue: { isAuthenticated: authenticated } }, { provide: Router, useValue: router }] });
  });

  it('redirects anonymous users to login', () => {
    const result = TestBed.runInInjectionContext(() => authGuard({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot));
    expect(result).toBeTruthy();
    expect(router.createUrlTree).toHaveBeenCalledWith(['/login']);
  });

  it('allows authenticated users through', () => {
    authenticated.set(true);
    const result = TestBed.runInInjectionContext(() => authGuard({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot));
    expect(result).toBeTrue();
    expect(router.createUrlTree).not.toHaveBeenCalled();
  });
});
