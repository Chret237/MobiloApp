# Mobilo API

Premier vertical slice du backend MVP : catalogue de véhicules avec SQLite et seed local.

## Lancer

Depuis la racine du dépôt :

```powershell
dotnet run --project backend/Mobilo.Api/Mobilo.Api.csproj --urls http://localhost:5080
```

La base `mobilo.db` est créée automatiquement au premier démarrage et contient les trois véhicules du prototype MAUI.

## Endpoints disponibles

- `GET /` : état du service
- `POST /api/auth/register` : créer un compte et retourner un bearer token
- `POST /api/auth/login` : ouvrir une session
- `GET /api/me` : profil protégé par bearer token
- `GET /api/vehicles` : catalogue paginé
- `GET /api/vehicles/{id}` : détail d'un véhicule

Paramètres de `GET /api/vehicles` : `search`, `category`, `maxPrice`, `page`, `pageSize`.

Exemple : `GET /api/vehicles?category=SUV&page=1&pageSize=20`

Les mots de passe sont hashés avec PBKDF2 et les tokens bearer sont signés avec la clé `Auth:SigningKey`. En développement, la clé est dans `appsettings.json` ; elle doit être remplacée par un secret d’environnement en production.

Les favoris, les devis, les réservations et le paiement simulé seront ajoutés dans les prochains slices.
