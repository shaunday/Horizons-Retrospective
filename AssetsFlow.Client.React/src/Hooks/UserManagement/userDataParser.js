export function parseUserData(dto) {
    if (!dto) return null;

    return {
        userId: dto.userId,
        symbols: dto.symbols ?? [],
        filters: dto.availableFilters ?? [],
    };
}
