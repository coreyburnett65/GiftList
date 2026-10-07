import { Alert, CircularProgress, Container, Stack, Typography } from '@mui/material';
import { usePing } from '../api/generated/diagnostics/diagnostics';

// Placeholder page: this only exists to prove OpenAPI -> orval -> axios -> TanStack Query
// -> MUI render all work together. Real pages replace this in the next implementation step.
export function HomePage() {
  const { data, isPending, isError } = usePing();

  return (
    <Container maxWidth="sm" sx={{ py: 6 }}>
      <Stack spacing={2}>
        <Typography variant="h4">Family Gift List</Typography>
        {isPending && <CircularProgress />}
        {isError && <Alert severity="error">Could not reach the API.</Alert>}
        {data && (
          <Alert severity="success">
            {data.message} (server time: {new Date(data.serverTimeUtc).toLocaleString()})
          </Alert>
        )}
      </Stack>
    </Container>
  );
}
