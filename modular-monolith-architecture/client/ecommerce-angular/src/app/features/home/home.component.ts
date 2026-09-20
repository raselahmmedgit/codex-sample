import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PageHeaderComponent } from '../../shared/components/page-header.component';

@Component({
  selector: 'app-home',
  imports: [RouterLink, PageHeaderComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="hero rounded-4 p-4 p-md-5 mb-5">
      <div class="row align-items-center g-4">
        <div class="col-lg-7">
          <span class="badge rounded-pill text-bg-light text-primary mb-3">NEW SEASON COLLECTION</span>
          <h1 class="display-5 fw-bold mb-3">Thoughtful products for everyday living.</h1>
          <p class="lead text-white-50 mb-4">Discover quality essentials, curated with clarity and delivered with care.</p>
          <a class="btn btn-light btn-lg px-4" routerLink="/products">Explore products</a>
        </div>
        <div class="col-lg-5">
          <div class="hero-card shadow-sm">
            <img class="hero-image" src="/images/product-catalog.png" alt="Curated collection featuring headphones, a mug and a backpack" />
            <div class="p-3"><span class="small text-secondary">CURATED PICK</span><strong class="d-block mt-2">Simple design. Better days.</strong></div>
          </div>
        </div>
      </div>
    </section>

    <app-page-header title="Shop with confidence" description="A clear, reliable shopping experience from discovery to delivery." />

    <section class="row g-4" aria-label="Store benefits">
      @for (benefit of benefits; track benefit.title) {
        <article class="col-md-4">
          <div class="benefit-card h-100 p-4 rounded-4 bg-white border">
            <div class="icon mb-3">{{ benefit.icon }}</div>
            <h2 class="h5">{{ benefit.title }}</h2>
            <p class="text-secondary mb-0">{{ benefit.description }}</p>
          </div>
        </article>
      }
    </section>
  `,
  styles: `
    .hero { overflow: hidden; color: white; background: linear-gradient(120deg, #172033, #3457a6); }
    .hero-card { min-height: 230px; overflow: hidden; border-radius: 1.25rem; background: #fff; color: #172033; text-align: center; }
    .hero-image { display: block; width: 100%; height: 190px; object-fit: cover; }
    .icon { display: grid; width: 2.75rem; height: 2.75rem; place-items: center; border-radius: .85rem; background: rgba(var(--bs-primary-rgb), .1); color: var(--bs-primary); font-size: 1.25rem; }
  `
})
export class HomeComponent {
  readonly benefits = [
    { icon: '✓', title: 'Verified quality', description: 'Every product is selected with practical quality and lasting value in mind.' },
    { icon: '↗', title: 'Fast fulfillment', description: 'Simple order tracking and dependable fulfillment keep you informed.' },
    { icon: '♡', title: 'Customer first', description: 'Helpful support and transparent policies at every step of your journey.' }
  ];
}
