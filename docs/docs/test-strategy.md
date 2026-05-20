# Test Strategy

## Мета тестування

Перевірити:
- коректність бізнес-логіки;
- стабільність системи;
- persistence;
- fault handling;
- інтеграцію між шарами.

## Типи тестів

### Unit Tests
Перевірка окремих класів:
- DateRange
- Reservation
- Strategy
- Repository

### Integration Tests
Перевірка взаємодії:
- BookingService + Repository
- JSON persistence
- LINQ queries
