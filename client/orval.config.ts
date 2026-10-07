import { defineConfig } from 'orval';

// Input is the OpenAPI document MSBuild writes at build time (OpenApiGenerateDocumentsOnBuild
// in GiftList.Api.csproj), so `dotnet build src/GiftList.Api` must run before `npm run generate-api`.
export default defineConfig({
  giftlist: {
    input: '../src/GiftList.Api/obj/GiftList.Api.json',
    output: {
      mode: 'tags-split',
      target: 'src/api/generated',
      client: 'react-query',
      httpClient: 'axios',
      override: {
        mutator: {
          path: 'src/lib/httpClient.ts',
          name: 'customInstance',
        },
      },
    },
  },
});
