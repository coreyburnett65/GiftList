# Family Gift List — Product Spec

## 1. Purpose

A private web app where people keep a wish list of gift ideas for themselves and share it with their family group. Group members can see each other's lists and mark items as purchased so nobody buys the same gift twice. The key rule: **you can never see which items on your own list have been purchased**, so gifts stay a surprise.

## 2. Key Concepts

- **User:** a person with an account. Each user has **exactly one wish list**.
- **Family group:** the set of people who buy gifts for each other (e.g. "Smith Family"). Every user belongs to **exactly one** family group. A user's list is visible to every other member of their group.
- **Managed child:** a user account for a young child, managed by one or more parent accounts. A managed child has one wish list and belongs to the same family group as their parents.

## 3. Roles

- **Member:** manages their own list, views and buys from the lists of other people in their group.
- **Group admin:** a member who can also invite and remove members and rename the group. The person who creates a group is its first admin.
- **Parent (manager):** a member who manages one or more managed child accounts. A child can have several parents (e.g. both parents). Each parent has full control of the child's list and can see its purchases.

## 4. Core Rules (must never be broken)

1. A user can add, edit, and delete items only on their own list.
2. A user can view the list of every other member of their family group, and no one outside it.
3. A user can mark an item on someone else's list as purchased. The app records who purchased it and when.
4. Every group member **except the list owner** can see each item's purchase status and who purchased it.
5. The list owner must never be able to discover purchase status through any route: not the UI, not API responses, not notifications, not item counts, not sort order, not AI suggestions. **This must be enforced on the server.** The API must strip purchase data from responses when the requester is the list owner. Hiding it only in the front end is not acceptable.
6. Only the person who marked an item purchased can un-mark it.
7. **Managed children are the exception to Rule 5.** On a managed child's list, the child's parents can add, edit, and delete items, see every item's purchase status and purchaser, and mark items as purchased themselves. The child is still treated as the list owner: if the child has their own login, the child never sees purchase status.

## 5. Features

### 5.1 Accounts and family groups
- Sign up and sign in with email and password (or a magic link).
- On sign-up, a user either creates a new family group or joins an existing one through an invite link sent by a group admin.
- A user who already belongs to a group can't join another one. To move, they must leave their current group first (see Edge Cases).
- Group admins can remove members, promote other members to admin, and rename the group.

### 5.2 My wish list
- One list per user, visible to everyone else in their group.
- Each item has: name (required), description, link to the product, price or price range, priority (low/medium/high), optional image URL, and notes such as size or color.
- Owner can add, edit, delete, and reorder items.

### 5.3 Marking gifts as received
Because each user keeps one ongoing list, received gifts need a way to leave it.
- The owner can mark any item on their list as **"Received"** at any time, for example after opening it at Christmas. Nothing is ever marked Received automatically.
- Received items move off the owner's active list and off the list everyone else sees, into the owner's **"Gifts received"** history.
- The Gifts received history shows the item and the date it was marked Received. It does **not** show whether anyone in the app purchased it or who did, so marking an item Received never reveals purchase information.
- The owner can move an item from Gifts received back to their active list (e.g. if they marked it by mistake).
- For managed children, parents mark items Received on the child's behalf.

### 5.4 Viewing and buying
- The home page shows the members of the user's family group, with upcoming birthdays and occasions first, and a link to each member's list.
- On someone else's list, each item shows **Available** or **Purchased by [name]**.
- "Mark as purchased" button on available items; "Undo" for the purchaser.
- A "My purchases" page shows everything the current user has bought, grouped by person, with a running total.

### 5.5 Occasions
- Each user's profile can include their birthday.
- A group can have shared occasions with dates (e.g. "Christmas 2026"), used for reminders and sorting.

### 5.6 Edge cases
- **Owner deletes an item that was already purchased:** do not reveal anything to the owner. Notify the purchaser that the item was removed.
- **Owner edits an item that was already purchased** (e.g. changes the size): flag the change to the purchaser.
- **Owner marks an item Received that someone purchased in the app:** notify the purchaser, so they know the owner considers it received (e.g. the owner got it from someone outside the app). Nothing is revealed to the owner.
- **Two people try to buy the same item at once:** the first to save wins; the second sees a message that it was just purchased.
- **A member leaves or is removed from the group:** purchases they made on other members' lists are removed (those items become Available again), and any purchases on their own list are cleared. Their list items stay with them. A managed child can't be removed separately from their parents' group; see 5.7.

### 5.7 Managed child accounts
- A parent creates a managed child account from their own account by entering the child's name and birthday. The child is automatically placed in the parent's family group, and the creating parent is the child's first manager.
- A parent can invite another member **of the same family group** to co-manage the child. The invited member must accept before they become a manager.
- A managed child does not need a login. Optionally, a parent can give the child a login so the child can view their own list and browse the group's lists. A child login can be set to view-only or allowed to edit their own list.
- A parent sees a "My kids" section listing each managed child, with links to edit the child's list and view its purchases.
- On a managed child's list, parents see an extra "Purchases" view showing each item's status, and who bought it. This lets parents coordinate, for example to see that Grandma already bought the bike.
- A parent can mark an item on their child's list as purchased, un-mark their own purchases, and mark items Received for the child.
- **Purchase notifications:** when someone marks an item on a managed child's list as purchased, every parent of that child is notified with the item and the purchaser's name. A parent who makes the purchase themselves is not notified. If the purchase is undone, parents are notified of that too. These notifications are never sent to the child, even if the child has a login.
- **Converting to independent:** any of the child's parents can convert the child from managed to independent at any time, using a "Convert to independent account" option in the child's settings. There is no age requirement and the app never prompts for it. The parent confirms the action and enters an email address for the child if one isn't already set, and the child receives an invitation to set their own password. After conversion, parents no longer manage the list, and the child becomes a normal owner who can't see purchases. Existing purchase records are kept.
- A child account can't be left without a manager. If the last parent tries to leave the group or delete their account, the app requires them to add another manager or convert the child first.

### 5.8 Gifter profiles
Each user can fill out an optional "About me" profile that helps others shop for them: birthday, interests and hobbies, clothing and shoe sizes, favorite colors, stores they like, and things they don't want or already have. The profile is visible to everyone in their group. Parents fill out profiles for managed children.

### 5.9 AI gift suggestions (Phase 3)
Uses the Claude API to suggest gift ideas.
- **For gifters:** on someone else's list, a "Suggest more ideas" button generates 5–10 ideas based on that person's list items, profile, and the upcoming occasion. Gifters can save an idea to a private "my ideas for [person]" scratchpad.
- **For the list owner:** a "Help me fill my list" button suggests items the owner can add to their own list. Parents can use this for their managed children.
- Suggestions for the list owner must be generated only from the owner's own items and profile and must never use purchase data.
- The Claude API key lives on the server only. All API calls go through the backend.
- Rate-limit suggestion requests per user to control cost.

## 6. Pages
1. Sign in / sign up
2. Create or join family group (join via invite link)
3. Home (members of the family group and upcoming occasions)
4. My wish list (edit mode, no purchase info)
5. Member's list (purchase mode)
6. My purchases
7. My kids (list of managed children, with links to their lists, purchases, and settings)
8. Managed child's list (edit mode plus purchases view, for parents)
9. Gifts received history
10. Profile / About me
11. Group settings (admins only)

## 7. Data Model (starting point)
- **User:** id, name, email (optional for managed children), group_id, role (member/admin), birthday, profile fields, is_managed, child_can_log_in, child_can_edit
- **Family group:** id, name, invite code
- **Guardianship:** parent_id, child_id, added_at (parent and child must have the same group_id)
- **Item:** id, owner_id, name, description, url, price, priority, image_url, notes, sort_order, status (active/received), received_at
- **Purchase:** id, item_id, purchaser_id, purchased_at (kept in its own table so it is easy to exclude from owner queries)
- **Occasion:** id, group_id, name, date
- **Saved idea:** id, author_id, for_user_id, text

Because each user has a single group_id, a viewer can see a list when the viewer and owner have the same group_id. A viewer can see purchases when they are in the same group and are not the owner, or when they are one of the owner's parents.

## 8. Tech Preferences

### 8.1 About the developer
I'm an experienced .NET developer (20+ years) and mostly a back-end developer. I'm building this as a **portfolio project to learn modern, cloud-based .NET web development**. The code should show current best practices that I could explain in an interview, not just code that works.

### 8.2 Stack
- **Back end:** ASP.NET Core on the current LTS release of .NET, in C#. Use Minimal APIs for endpoints and Entity Framework Core for data access.
- **Front end:** a React single-page app written in TypeScript, built with Vite. I'm choosing React because it is widely used and in demand, so the front end should follow current mainstream React practices an employer would expect to see.
  - Use an established component library (e.g. MUI, or shadcn/ui with Tailwind) so the UI looks polished without much custom CSS. Recommend one in the plan.
  - Use React Router for routing, TanStack Query for fetching and caching API data, and React Hook Form with Zod for forms and validation.
  - Generate the TypeScript API client from the back end's OpenAPI document, so front-end types always match the API.
- **Database:** Azure SQL Database or Azure Database for PostgreSQL. Recommend one in the plan, keeping cost low.
- **Authentication:** ASP.NET Core Identity or Microsoft Entra External ID. Recommend one in the plan and explain the tradeoffs, including how each handles managed child accounts that have no login. Prefer a backend-for-frontend (BFF) approach with secure, HTTP-only cookies over storing tokens in the browser, and explain the reasoning.
- **Hosting:** Azure. The API on App Service or Container Apps; the React app either served by the ASP.NET Core host or deployed to Azure Static Web Apps. Recommend one in the plan, considering cost and the BFF/cookie setup. Prefer free or low-cost tiers.
- **Local development:** Aspire for orchestrating the API, the React dev server, and the database locally, with a local database in a container.
- **Infrastructure as code:** Bicep, deployable with the Azure Developer CLI (`azd`).
- **CI/CD:** GitHub Actions that build, run tests, and deploy.
- **Secrets:** Azure Key Vault (or App Service settings backed by Key Vault) for the Claude API key and connection strings. No secrets in source control.
- **Observability:** OpenTelemetry with Application Insights.
- **AI:** call the Claude API from the back end only. Use Anthropic's official C# SDK if one is available and suitable; otherwise use a typed `HttpClient`.

### 8.3 Architecture and code quality
- Keep the purchase-visibility rules from Section 4 in one place in the back end (e.g. a service or query layer that every endpoint goes through), so they can't be bypassed by a new endpoint. Because the React app runs in the browser, anything the API returns can be seen by the user, so the API must never send purchase data to a list owner. The front end must not be relied on to hide anything.
- Design the API as a clean, documented REST API with ASP.NET Core's built-in OpenAPI support.
- Moderate layering is fine (e.g. separate projects for API/web, application logic, and data), but avoid ceremony that a small app doesn't need. Explain any pattern you choose.
- Use dependency injection, async throughout, nullable reference types, and the options pattern for configuration.
- Use EF Core migrations for schema changes.

### 8.4 Testing
- Back-end unit tests with xUnit.
- Front-end component tests with Vitest and React Testing Library.
- A small set of end-to-end tests with Playwright covering the main flows (sign in, add an item, buy an item from someone else's list, confirm the owner doesn't see it).
- Integration tests using `WebApplicationFactory` against a real database in a container (e.g. Testcontainers).
- Integration tests must cover every visibility rule in Section 4, including managed-child cases (parents see and can mark purchases on the child's list; a child with a login never sees them; access ends when a child is converted to an independent account) and users outside the group seeing nothing.

### 8.5 Working style for Claude Code
- Since this is a learning project, briefly explain modern .NET and Azure features as you introduce them, especially where they differ from older .NET Framework or early ASP.NET Core practices.
- React and TypeScript are newer to me, so explain React concepts (components, hooks, state, effects, data fetching) as they come up, and point out common mistakes to avoid.
- Before adding a new library, package, or Azure service, explain what it is for and ask first.
- I'm comfortable with back-end code, so keep back-end explanations short. Explain front-end and UI decisions in more detail.
- Keep the front end idiomatic React rather than writing it in a C# style.

## 9. Look and Feel
Warm, friendly, and uncluttered. Large tap targets, readable on a phone, light holiday touches without being seasonal-only.

## 10. Build Phases
- **Phase 1:** solution structure, Aspire local setup, database and migrations, React app scaffold, CI pipeline, accounts, family group with invites, own list with full add/edit/delete.
- **Phase 1b:** first deployment to Azure with Bicep and `azd`, so the app is live early.
- **Phase 2:** viewing others' lists, mark as purchased, mark as received, My purchases, Gifts received, hidden-purchase rules with tests, managed child accounts with co-parents.
- **Phase 3:** profiles and AI gift suggestions.
- **Phase 4:** child logins, converting a child to an independent account, occasions, notifications (reminders, purchased-item-changed alerts, parent purchase notifications).

## 11. Out of Scope (for now)
- Buying through the app or payment processing
- Native iOS/Android apps
- Public lists or sharing outside the family group
- Price tracking or automatic product scraping

## 12. Possible Future Features
These are not part of the current build. Keep the design flexible enough that they can be added later without major rework.

- **Multiple family groups:** a user can belong to more than one group (e.g. their own family, their spouse's family, coworkers) while keeping a single wish list.
- **Surprise items:** gifters can add items to someone else's list that the owner can't see. Other group members see them, so surprise gifts are coordinated and not duplicated.
- **Split gifts:** two or more people can go in together on one item. Each contributor is recorded, and the item shows as purchased with all contributors' names (hidden from the owner, like any purchase).
