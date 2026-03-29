# Squash Commits (Same Branch, No New Branch)

## Steps

```bash
# 1. See how many commits to squash
git log --oneline master..HEAD

# 2. Soft reset — keeps all changes staged, rewinds commits
git reset --soft master

# 3. Re-commit as a single commit
git commit -m "Your single commit message here"

# 4. Force push (same branch, no new branch needed)
git push --force-with-lease
```

## Notes

- `git reset --soft` rewinds commits but keeps all file changes staged — nothing is lost
- `--force-with-lease` is safer than `--force` — it fails if someone else pushed to the branch since your last fetch
- The PR updates automatically after force push
- You can also use `git reset --soft HEAD~N` to squash only the last N commits instead of all commits since master

## Alternative: ADO Merge Strategy

When completing a PR in Azure DevOps, select **"Squash commit"** as the merge type. This squashes all commits into one on merge without rewriting branch history — no force push needed.
