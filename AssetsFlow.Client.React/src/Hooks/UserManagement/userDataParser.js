export function parseUserData(dto) {
    if (!dto) return null;

    return {
        userId: dto.UserId,
        symbols: dto.Symbols ?? [],
        filters: dto.AvailableFilters ?? [],
    };
}
